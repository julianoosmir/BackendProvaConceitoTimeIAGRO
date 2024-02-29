﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using WebLivros.Interfaces;
using WebLivros.Models;

namespace WebLivros.Services
{
    public class BookService : IBookService
    {
        public BookService() { }

        public List<Book> GetBooks()
        {
            return DeserializarJson();

        }
        private List<Book> DeserializarJson()
        {
            StreamReader r = new StreamReader("C:\\Users\\julia\\Documents\\miltec\\BackendProvaConceitoTimeIAGRO\\books.json");
            string jsonString = r.ReadToEnd();
            var m = JsonConvert.DeserializeObject<IEnumerable<Book>>(jsonString);

            List<Book> result = m.ToList();

            foreach (var item in result)
            {

                if (item.specifications.Illustrator is JArray)
                {
                    item.specifications.Illustrator = JarrayToList((JArray)item.specifications.Illustrator);
                }

                if (item.specifications.Genres is JArray)
                {
                    item.specifications.Genres = JarrayToList((JArray)item.specifications.Genres);
                }


            }

            return result;

        }
        private string[] JarrayToList(JArray jArray)
        {
            return jArray.ToObject<string[]>();
        }

        public List<Book> GetByProperty(string parametros)
        {
            List<Book> result = GetBooks();
            List<Book> query = result.Where(res => res.specifications.Illustrator.ToString().Contains(parametros)).ToList();
            return query;
        }

        public List<Book> GetByName(string name)
        {
            List<Book> result = GetBooks();
            List<Book> query = result.Where(res => res.Name.ToLower().Contains(name.ToLower())).ToList();
            return query;
        }

        public List<Book> GetByAuthor(string name)
        {
            List<Book> result = GetBooks();
            List<Book> query = result.Where(res => res.specifications.Author.ToLower().Contains(name.ToLower())).ToList();
            return query;
        }

        public List<Book> GetByAsc()
        {
            List<Book> result = GetBooks();
            List<Book> query = result.OrderBy(res => res.price).ToList();
            return query;
        }
        public List<Book> GetByDesc()
        {
            List<Book> result = GetBooks();
            List<Book> query = result.OrderByDescending(res => res.price).ToList();
            return query;
        }
        public double GetValueFrete(int id)
        {
            List<Book> result = GetBooks();
            var valor = result.FirstOrDefault(res => res.Id == id).price;
            var frete = valor * 0.2;

            return Math.Round(valor + frete,2);
        }

    }
}