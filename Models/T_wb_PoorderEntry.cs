using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace WebApp.Model
{
    public class T_wb_PoorderEntry
    {
        /// <summary>
        /// 物料代码
        /// </summary>
        public string? FItemNo { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal? FQty { get; set;}
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? FPrice { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal? FAmount { get;set; }
        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime? FJHDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? FRemark { get; set; }
        /// <summary>
        /// 内码
        /// </summary>
        [Key]
        public int FID { get; set; }
        /// <summary>
        /// 行号
        /// </summary>
        public int? FIndex { get; set; }
    }
}
