using System;
using System.ComponentModel.DataAnnotations;

namespace ETicaretData.ViewModels
{
	public class ShippingDetails
	{
		[Required(ErrorMessage = "Lütfen boş geçmeyiniz!")]
		public string UserName { get; set; }

        [Required(ErrorMessage = "Lütfen boş geçmeyiniz!")]
        public string AddressTitle { get; set; }

        [Required(ErrorMessage = "Lütfen boş geçmeyiniz!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Lütfen boş geçmeyiniz!")]
        public string City { get; set; }

        [Required(ErrorMessage = "Lütfen boş geçmeyiniz!")]
        public string Email { get; set; }

		public ShippingDetails()
		{
		}
	}
}

