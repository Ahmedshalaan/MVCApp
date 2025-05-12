using Demo.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Common.Services.EmailSettings
{
	public class EmailSetting : IEmailSetting
	{
		public void SendEmail(Email email)
		{
			// mailServer => gmail.com

			var Client = new SmtpClient("smtp.gmail.com", 587);

			Client.EnableSsl = true;

			// Sender , Reciver
			// User who Tries To Reset Password

			Client.Credentials = new NetworkCredential("doaaamin.route@gmail.com", "uzknlqivbrinqnki") ; // Generate Application Password

			// Reciver

			Client.Send("doaaamin.route@gmail.com", email.To, email.Subject, email.Body);
		}
	}
}
