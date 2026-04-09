//Inventory Transaction (Giao dịch tồn kho) là các hoạt động ghi nhận sự thay đổi về số lượng hoặc giá trị hàng hóa trong kho, bao gồm nhập (receipts), xuất (issues), chuyển kho (transfers) hoặc điều chỉnh (adjustments).
namespace CoffeeShop.Models.Entities.Inventory
{
    public class InventoryTransaction 
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int IngredientId { get; set; }
        public int QuantityChange { get; set; }
        public string TransactionType { get; set; }
        public string Reason { get; set; }
        //Để tí nữa Join hai bảng dễ dàng và tối ưu hơn
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual Store Store { get; set; }
        public virtual Ingredient Ingredient{ get; set; }
        public virtual Auth.User User { get; set; }
    }
}