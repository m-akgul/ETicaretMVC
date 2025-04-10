using System;
namespace ETicaretData.ViewModels
{
	public class UserRoleViewModel
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public UserRoleViewModel()
		{
		}
	}
}

