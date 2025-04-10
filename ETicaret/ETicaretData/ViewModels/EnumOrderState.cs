using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
namespace ETicaretData.ViewModels
{
	public enum EnumOrderState
	{
        [Display(Name = "Onay Bekliyor")]
        Waiting,
        [Display(Name = "Onaylandı")]
        Approved,
        [Display(Name = "İptal Edildi")]
        Canceled,
        [Display(Name = "Tamamlandı")]
        Completed

    }
}

