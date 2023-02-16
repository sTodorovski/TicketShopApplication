﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System;
using TicketShop.Domain.DomainModels;
using GemBox.Document;
using ClosedXML.Excel;
using System.Linq;

namespace TicketShopApplication.Controllers
{
    public class OrderController : Controller
    {
        public OrderController()
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        public IActionResult Index()
        {
            HttpClient client = new HttpClient();

            string URL = "https://localhost:44308/api/Admin/GetAllActiveOrders";

            HttpResponseMessage response = client.GetAsync(URL).Result;

            var data = response.Content.ReadAsAsync<List<Order>>().Result;

            return View(data);
        }

        public IActionResult Details(Guid id)
        {
            HttpClient client = new HttpClient();

            string URL2 = "https://localhost:44308/api/Admin/GetDetailsForOrder";

            var model = new
            {
                Id = id,
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = client.PostAsync(URL2, content).Result;

            var result = responseMessage.Content.ReadAsAsync<Order>().Result;

            return View(result);
        }

        [HttpGet]
        public FileContentResult ExportAllOrders()
        {
            string fileName = "Orders.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Orders");

                worksheet.Cell(1, 1).Value = "Order Id";
                worksheet.Cell(1, 2).Value = "Customer Email";


                HttpClient client = new HttpClient();


                string URL = "https://localhost:44308/api/Admin/GetAllActiveOrders";

                HttpResponseMessage responseMessage = client.GetAsync(URL).Result;

                var result = responseMessage.Content.ReadAsAsync<List<Order>>().Result;

                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];

                    worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.User.Email;

                    for (int p = 0; p < item.Tickets.Count(); p++)
                    {
                        worksheet.Cell(1, p + 3).Value = "Ticket #" + (p + 1);
                        worksheet.Cell(i + 1, p + 3).Value = item.Tickets.ElementAt(p).SelectedTicket.Movie;
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }

            }
        }

        public FileContentResult CreateInvoice(Guid id)
        {
            HttpClient client = new HttpClient();

            string URL2 = "https://localhost:44308/api/Admin/GetDetailsForOrder";

            var model = new
            {
                Id = id,
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = client.PostAsync(URL2, content).Result;

            var result = responseMessage.Content.ReadAsAsync<Order>().Result;

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{OrderNumber}}", result.Id.ToString());
            document.Content.Replace("{{UserName}}", result.User.Korisnik);

            StringBuilder sb = new StringBuilder();

            var totalPrice = 0;

            foreach (var item in result.Tickets)
            {
                totalPrice += item.Quantity * item.SelectedTicket.Price;
                sb.AppendLine(item.SelectedTicket.Movie + " with a number of: " + item.Quantity + " and the total price of: " + item.SelectedTicket.Price + " MKD");
            }

            document.Content.Replace("{{TicketList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", totalPrice.ToString() + " MKD");

            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "Invoice.pdf");
        }
    }
}
