using System.ComponentModel.DataAnnotations;
namespace Frontek_Full_Web_E_Commerce.Models
{
    public class OrdenDetalle
    {
        [Key]
        public int OrdenDetalleId { get; set; }
        public int OrdenId { get; set; }
        public virtual Orden Orden { get; set; }
        public string NombreProducto { get; set; }
        [StringLength(50)]
        public string SKU { get; set; }
        [Required]
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        [StringLength(200)]
        public string Garantia { get; set; }

        //fk
        public int ProductoId { get; set; } = 1;

        public virtual Producto Producto{ get; set; }
    }
}