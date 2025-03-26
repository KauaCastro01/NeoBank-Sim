using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoBank_Sim
{
    class Program : Usuario
    {
        //Metodo Main
        static void Main(string[] args)
        {
            Console.Title = "NeoBank Sim";
            Program p = new Program();
            p.MenuInicial();
        }
        //Metodo para exibir o menu inicial
        public void MenuInicial()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Bem vindo ao NeoBank Sim!");
                Console.WriteLine("1 - Login");
                Console.WriteLine("2 - Cadastro");
                Console.WriteLine("X - Sair");
                Console.Write("Digite a opção: ");
                var opcao = Console.ReadLine().ToLower();

                switch (opcao)
                {
                    case "1": Login(); break;
                    case "2": Cadastro(); break;
                    case "x": Console.Write("\nNeoBank Sim Encerrado!"); Environment.Exit(0); break;
                    default: Console.Write("\a\nOpção inválida!"); Console.ReadKey(); break;
                }
            }
        }
    }
}
