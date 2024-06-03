using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class City:Entity
    {
     //   public int Id { get; set; }
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        //    public ICollection<MatrixCity> MatrixCities { get; set; }
    }
    public class InputTable : Entity
    {
        [Key]
 
        public int CityCount { get; set; }
        public int RouteCount { get; set; }
        public string CoordinatesMatrix { get; set; } // Представление матрицы координат в строке, например, JSON
        public int DisruptionPercentage { get; set; }
    }

    public class ResultTable : Entity
    {
        [Key]
    
        public int OptimalValue { get; set; }
        public string OptimalRoutes { get; set; } // Представление маршрутов в строке, например, JSON
        public int InputTableId { get; set; } // Внешний ключ
        public InputTable InputTable { get; set; }
    }
}
