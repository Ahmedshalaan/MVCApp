using Demo.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Common.Services.EmailSettings
{
	public interface IEmailSetting
	{
		public void SendEmail(Email email);
	}
}
