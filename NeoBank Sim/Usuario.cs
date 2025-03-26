using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoBank_Sim
{
    class Usuario : ConexaoSQL
    {
        //Propriedades
        public string nomeCompleto { get; set; }
        public string cpf { get; set; }
        public string senha { get; set; }
        //Metodo para realizar o login
        public void Login()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("NeoBank Sim - Menu de login");
                Console.WriteLine("Digite 'sair' para voltar para o menu");
                Console.WriteLine("Prencha os dados abaixo:\n");
                Console.Write("Digite o CPF: ");
                cpf = Console.ReadLine().ToLower();
                if(cpf == "sair") { Program p = new Program(); p.MenuInicial(); }
                if (cpf.Length == 11)
                {
                    try { var cpfDigitos = decimal.Parse(cpf); }
                    catch { Console.Write("\a\nNo CPF informado possui letras!!"); Console.ReadKey(); continue; }
                    Console.Write("Digite sua senha: ");
                    senha = Console.ReadLine();
                    if (senha.Length >= 6 && senha.Length <= 30) { EfetuarLogin(cpf, senha); }
                    else { Console.WriteLine("\a\nA senha deve possuir de 6 a 30 caracteres!"); Console.ReadKey(); }
                }
                else { Console.Write("\a\nCPF quantidade de digitos inválido, tente novamnete!"); Console.ReadKey(); }
            }
        }
        //Metodo para realizar o cadastro
        public void Cadastro()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("NeoBank Sim - Menu de cadastro");
                Console.WriteLine("Digite 'sair' para voltar para o menu");
                Console.WriteLine("Prencha os dados abaixo:\n");
                Console.Write("Digite seu nome completo: ");
                nomeCompleto = Console.ReadLine();
                if (nomeCompleto == "sair") { Program p = new Program(); p.MenuInicial(); }
                if (nomeCompleto.Length >= 4 && nomeCompleto.Length <= 100)
                {
                    Console.Write("Digite o CPF: ");
                    cpf = Console.ReadLine().ToLower();
                    if (cpf.Length == 11)
                    {
                        try { var cpfDigitos = decimal.Parse(cpf); }
                        catch { Console.Write("\a\nNo CPF informado possui letras!!"); Console.ReadKey(); continue; }
                        Console.Write("Digite sua senha: ");
                        senha = Console.ReadLine();
                        if (senha.Length >= 6 && senha.Length <= 30) { EfetuarCadastro(nomeCompleto, cpf, senha); }
                        else { Console.WriteLine("\a\nA senha deve possuir de 6 a 30 caracteres!"); Console.ReadKey(); }
                    }
                    else { Console.Write("\a\nCPF quantidade de digitos inválido, tente novamnete!"); Console.ReadKey(); }
                }
                else { Console.WriteLine("\a\nO nome deve possuir de 4 a 100 caracteres"); Console.ReadKey(); }
            }
        }
    }
}
