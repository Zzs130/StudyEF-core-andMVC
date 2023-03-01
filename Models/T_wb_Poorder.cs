using System.ComponentModel.DataAnnotations;

namespace WebApp.Model
{
    public class T_wb_Poorder
    {
        /// <summary>
        /// 供应商
        /// </summary>
        public string? FCustNo { get; set; }
        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime FDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? FRemark { get; set;}
        /// <summary>
        /// 内码
        /// </summary>
        [Key]
        public int FID { get; set;}
        /// <summary>
        /// 单据编码
        /// </summary>
        public string? FBillNo { get; set;}
    }
}
