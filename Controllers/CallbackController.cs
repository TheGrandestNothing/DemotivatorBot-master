using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace DemotivatorBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        /// <summary>
        /// Конфигурация приложения
        /// </summary>
        private readonly IConfiguration _configuration;
        private readonly IVkApi _vkApi;
        public CallbackController(IVkApi vkApi, IConfiguration configuration)
        {
            _vkApi = vkApi;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Callback([FromBody] Updates updates)
        {
            // Проверяем, что находится в поле "type" 
            switch (updates.Type)
            {
                // Если это уведомление для подтверждения адреса
                case "confirmation":
                    // Отправляем строку для подтверждения 
                    return Ok(_configuration["Config:Confirmation"]);
                case "message_new":
                {
                    // Десериализация
                    var msg = Message.FromJson(new VkResponse(updates.Object));
                    var uploadServer = _vkApi.Photo.GetMessagesUploadServer(msg.PeerId.Value);
                    var wc = new WebClient();
                    var fileName = "";
                    foreach(var attach in msg.Attachments)
                    {
                        if (attach.Type == typeof(Photo))
                        {
                            if(attach.Instance is Photo ph)
                            {
                                fileName = ph.GetHashCode().ToString() + ".jpg";
                                wc.DownloadFile(ph.PhotoSrc, fileName);
                                break;
                            }
                        }
                    }
                    if (fileName != "")
                    {
                        var demProc = new DemotivatorGenerator.Processing.DemotivatorProcessor("ДИМАТИВАТОР БОТ","ЧТО ДАЛЬШЕ?");
                        var demimg = demProc.Process((Bitmap)Bitmap.FromFile(fileName));
                        demimg.Save("proc_" + fileName);
                        var result = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, "proc_" + fileName));
                        var photo = _vkApi.Photo.SaveMessagesPhoto(result);

                        _vkApi.Messages.Send(new MessagesSendParams
                        {
                            UserId = msg.PeerId,
                            Message = "сообщение",
                            Attachments = new List<MediaAttachment>
                            {
                                photo.FirstOrDefault() //берем первое фото из коллекции.
                            }
                        });
                    }
                        // Отправим в ответ полученный от пользователя текст
                    break;
                }
            }
            // Возвращаем "ok" серверу Callback API
            return Ok("ok");
        }
    }
}