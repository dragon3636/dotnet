namespace Model.EF
{ 
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        public long ID { get; set; }

        [StringLength(250)]
        [Display(Name="Tên sản phẩm")]
        public string Name { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(250)]
        public string MetaTitle { get; set; }

        [StringLength(500)]
        [Display(Name="Mô tả")]

        public string Descriptions { get; set; }

        [StringLength(250)]
        [Display(Name="Hình ảnh")]
        public string Image { get; set; }

        [Column(TypeName = "xml")]
        public string MoreImage { get; set; }
        [Display(Name = "GIá")]
        public decimal? Price { get; set; }
        [Display(Name = "Giá ưu đãi")]
        public decimal? PromotionPrice { get; set; }
        [Display(Name = "Tính thuế VAT")]
        public bool? IncludedVAT { get; set; }
        [Display(Name = "Số lượng")]
        public int? Quantity { get; set; }
        [Display(Name = "Thuộc danh mục")]
        public long? CategoryId { get; set; }
    
        [Column(TypeName = "ntext")]
        [Display(Name = "Chi tiết")]
        public string Detail { get; set; }

        public int? Warranty { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(50)]
        public string CreateBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        [StringLength(50)]
        public string MetaKeywords { get; set; }

        [StringLength(10)]
        public string MetaDescriptions { get; set; }
        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }
        public DateTime? TopHot { get; set; }

        public int? ViewCount { get; set; }
    }
}
