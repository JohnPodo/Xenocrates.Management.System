using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementSystemVersionTwo.Models
{
    public class ChatHub : Hub, IDisposable
    {
        private ApplicationDbContext _context;
        public ChatHub()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public void Send(string name, string message)
        {
            Message msg = new Message() { Name = name, Text = message, Date = DateTime.Now };
            var department = _context.Workers.SingleOrDefault(x => (x.FirstName + " " + x.LastName) == name).Department;
            msg.Department = department;
            _context.Messages.Add(msg);
            _context.SaveChanges();
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }
    }
}