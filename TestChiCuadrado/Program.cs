using MathNet.Numerics.Distributions;
using MathNet.Numerics.Integration;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestChiCuadrado
{
    internal class Program
    {
        /*Deben agregar los aleatorios solo numeros dentro de -> '/debub/data.txt'     */
        /*Instalar:              Install-Package MathNet.Numerics      */
        static string ruta = "data.txt";
        static void Main(string[] args)
        {
            
            int Ntotal = ContarLineas(ruta);
            double alfa = 0.05;
            double gradosLiberta = 0;
            double n = Math.Sqrt(Ntotal);
            double AC = 1/(Ntotal/n);
            double[] vector = LeerVector(ruta);
            Array.Sort(vector);
            double li = vector[0], lf=li+AC;
            double E = Ntotal / n;
            double sumatoriaEstadistica = 0; double contador = 0;
            double verificadorTotalidad = 0;
   
            do
            {

                for (int i = 0; i < vector.Length; i++)
                {
                    double dato = vector[i];
                    if (dato >= li && dato < lf)
                    {
                        contador++;
                    }
                    
                }
               
                li = lf;
                lf = lf + AC;
                verificadorTotalidad += contador;
                sumatoriaEstadistica += ((Math.Pow((contador - E), 2)) / E);
         
                contador = 0; if (verificadorTotalidad == Ntotal) break;
                gradosLiberta++;
                
            }
            while (lf<1);
       
            if(verificadorTotalidad<Ntotal) sumatoriaEstadistica += ((Math.Pow(((Ntotal-verificadorTotalidad) - E), 2)) / E);


            double chiCritico = ChiSquared.InvCDF(gradosLiberta, 1 - alfa);
            

            Console.WriteLine(sumatoriaEstadistica<chiCritico ? "SE ACEPTA":"SE RECHAZA");
        }
        static double[] LeerVector(string rutaArchivo)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo))
                throw new ArgumentException("La ruta no puede estar vacía.");
            if (!File.Exists(rutaArchivo))
                throw new FileNotFoundException("Archivo no encontrado.", rutaArchivo);

            var lista = new List<double>();
            using (var sr = new StreamReader(rutaArchivo))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                {
                    double valor;
                    if (double.TryParse(linea, out valor))
                        lista.Add(valor);
                    else
                        throw new FormatException($"La línea '{linea}' no es un número válido.");
                }
            }
            return lista.ToArray();
        }

        static int ContarLineas(string rutaArchivo)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo))
                throw new ArgumentException("La ruta no puede estar vacía.");
            if (!File.Exists(rutaArchivo))
                throw new FileNotFoundException("Archivo no encontrado.", rutaArchivo);
            int contador = 0;
            using (var sr = new StreamReader(rutaArchivo))
                while (sr.ReadLine() != null)
                    contador++;
            return contador;
        }
    }
}
