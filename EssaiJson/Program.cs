using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EssaiJson
{
    class Program
    {
        static void Main(string[] args)
        {
            // Adresse du service google map
            string uri = "http://maps.googleapis.com/maps/api/geocode/json?latlng=39.28,-76.60&sensor=false";

            // Création de la requète
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(uri));
            request.ContentType = "application/json";
            request.Method = "GET";
            // Récupération des données JSON
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();

            // Stockage des données dans un objet JSON
            JsonTextReader reader = new JsonTextReader(new StreamReader(stream));
            JObject json = (JObject)JToken.ReadFrom(reader);

            // Ville
            string ville = "Inconnu";

            // Si la requete est ok on récupère les données
            if (((string)json["status"]).Equals("OK"))
            {
                // Récupération des lieux
                JToken lieux = json["results"][0]["address_components"];

                // on parcours les résultats
                foreach (JToken lieu in lieux)
                { 
                    // Stockage des types dans un tableaux
                    JArray types = (JArray)lieu["types"];
                    
                    // Si le type existe dans ce résultat et qu'il indique une localité
                    if (types != null && ((string)types[0]).Equals("locality"))
                    {
                        // On récupère le premier short_name de la localité
                        ville = (string)lieu["short_name"];
                    }
                }
            }

            Console.Out.WriteLine("Ville : " + ville);

            Console.ReadLine();
        }
    }
}
