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
        static int Ntotal=0;
        static void Main(string[] args)
        {

            double alfa = 0.05;

           
            double[] vector = LeerVector(ruta);
            Array.Sort(vector);

            
            double n = Math.Sqrt(Ntotal);
            double AC = 1.0 / (Ntotal / n);
            double E = Ntotal / n;

          
            double li = vector[0];
            double lf = li + AC;
            double sumatoriaEstadistica = 0;
            double verificadorTotalidad = 0;
            int gradosLiberta = 0;
            while ( li < 1.0)
            {
                int contador = 0;
                foreach (var dato in vector)
                {
                    if (dato >= li && dato < lf)
                        contador++;
                }

                verificadorTotalidad += contador;
                sumatoriaEstadistica += Math.Pow(contador - E, 2) / E;

                
                li = lf;
                lf += AC;
                if (verificadorTotalidad == Ntotal) break;
                gradosLiberta++;
            }
          
            //Si en la tabla estadistica nos falto un número para completar el "Ntotal", entonces, se cálcula.
            if (verificadorTotalidad < Ntotal)
            {
                int resto = Ntotal - (int)verificadorTotalidad;
                sumatoriaEstadistica += Math.Pow(resto - E, 2) / E;
            }

            double chiCritico = ChiSquared.InvCDF(gradosLiberta, 1 - alfa);
          

            Console.WriteLine(sumatoriaEstadistica<chiCritico ? "NO SE RECHAZA":"SE RECHAZA");
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
                    {
                        lista.Add(valor);
                        Ntotal++;
                    }
                       
                    
                }
            }
            return lista.ToArray();
        }

    }
}
