

namespace BiblioTech.Domain.Models
{
    public class DataResults<TData>
    {
        public DataResults()
        {

            this.Sucess = true;
        }

        /// <summary>
        /// Sucess y Message son los resultados que puede resultar de una operación
        /// </summary>
        public bool Sucess { get; set; }
        public string Message {get; set;}

        /// <summary>
        /// Este TData es el dato generico que retorna
        /// </summary>
        public TData Result {get; set;}
    }
}
