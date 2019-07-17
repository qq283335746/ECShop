using System;

namespace TygaSoft.Model
{
    [Serializable]
    public class ProductInfo
    {
        #region 成员方法

        public string ProductId { get; set; }
        public string CategoryId { get; set; }
        public string ProductName { get; set; }
        public string Subtitle { get; set; }
        public decimal ProductPrice { get; set; }
        public string ImagesUrl { get; set; }
        public DateTime CreateDate { get; set; }
        public object UserId { get; set; }
        public string PNum { get; set; }
        public int StockNum { get; set; }
        public string ImagesAppend { get; set; }
        public string MainImage { get; set; }
        public string LImagesUrl { get; set; }
        public string MImagesUrl { get; set; }
        public string SImagesUrl { get; set; }
        public decimal MarketPrice { get; set; }
        public string PayOptions { get; set; }
        public int ViewCount { get; set; }
        public string CustomAttrs { get; set; }
        public string Descr { get; set; }

        #endregion
    }
}