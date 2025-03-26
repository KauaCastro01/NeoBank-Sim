using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace NeoBank_Sim
{
    class variaveisConta
    {
        public string nomeCompleto { get; set; }
        public string nome { get; set; }
        public string cpf { get; set; }
        public decimal saldo { get; set; }
        public decimal fatura { get; set; }
        public decimal emprestimo { get; set; }
        public decimal limite { get; set; }
        public string idConta { get; set; }
        public uint agencia { get; set; }
        public int cartao { get; set; }
        public string numeroCartao { get; set; }

        public variaveisConta() { }
        public variaveisConta(string nomeCompleto, string nome, string cpf, decimal saldo, decimal fatura, decimal emprestimo, uint limite, string idConta, string numeroCartao, int cartao)
        {
            this.nomeCompleto = nomeCompleto;
            this.nome = nome;
            this.cpf = cpf;
            this.saldo = saldo;
            this.fatura = fatura;
            this.emprestimo = emprestimo;
            this.limite = limite;
            this.idConta = idConta;
            this.numeroCartao = numeroCartao;
            this.cartao = cartao;
            menuPrincipal();
        }
        //Metodo para menus não disponiveis
        public void nd()
        {
            Console.WriteLine("\a\nNão Disponivel Ainda, Em Breve Nova Atualização!");
            Console.ReadKey();
        }

        //Metodo do menu principal
        public void menuPrincipal()
        {
            DateTime dateTime = DateTime.Now;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("NeoBank Sim - Menu Principal");
                Console.WriteLine($"Data: {dateTime.Day}|{dateTime.Month}|{dateTime.Year}\n");
                Console.WriteLine($"Usuário Conectado: {nome}");
                Console.WriteLine($"Saldo: {saldo:F2}R$");
                Console.WriteLine($"Fatura: {fatura:F2}R$");
                Console.Write((emprestimo > 0 ? $"\nEmprestimo: {emprestimo:F2}R$\n" : ""));
                Console.Write((dateTime.Day >= 1 && fatura > 0 && dateTime.Day < 10 && fatura > 0 ? "A fatura está em aberto\n" : ""));
                Console.WriteLine("\nMenu Principal:");
                Console.WriteLine("1 - Transferencia");
                Console.WriteLine("2 - Fatura");
                Console.WriteLine("3 - Historico de transações");
                Console.WriteLine("4 - Pagar contas");
                Console.WriteLine("5 - Emprestimo");
                Console.WriteLine("6 - Limite");
                Console.WriteLine("7 - Cartões");
                Console.WriteLine("8 - Conta");
                Console.WriteLine("9 - Configurações");
                Console.WriteLine("X - Sair");
                Console.Write("Digite aqui: ");
                var opcao = Console.ReadLine().ToLower();
                switch (opcao)
                {
                    case "1": transferencia(); break;
                    case "2": MenuFatura(); break;
                    case "3": HistoricoTransacoes(); break;
                    case "4": nd(); break;
                    case "5": MenuEmprestimo(); break;
                    case "6": MenuLimite(); break;
                    case "7": MenuCartao(); break;
                    case "8": MenuConta(); break;
                    case "9":nd(); break;
                    case "x": Console.WriteLine("NeoBank Sim Encerrado!"); Environment.Exit(0); break;
                    default: Console.WriteLine("\a\nOpção inválida"); Console.ReadKey(); break;
                }
            }
        }
        //Metodo do menu de transferencia
        public void transferencia()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("NeoBank Sim - Central de transferencia");
                Console.WriteLine("\nO banco do destinatario é: ");
                Console.WriteLine("1 - NeoBank Sim");
                Console.WriteLine("2 - Outro banco");
                Console.WriteLine("X - Sair");
                Console.Write("Digite a opção: ");
                var opcao = Console.ReadLine().ToLower();

                switch (opcao)
                {
                    case "1": transferencia(1); break;
                    case "2": transferencia(2); break;
                    case "x": menuPrincipal(); break;
                    default: Console.WriteLine("\a\nOpção inválida"); Console.ReadKey(); break;
                }
            }
        }
        //Metodo para realizar a transferencia
        public void transferencia(int i)
        {
            FuncoesBanco fb = new FuncoesBanco();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("NeoBank Sim - Transferencia");
                Console.WriteLine("Digite 'sair' para voltar para o menu");
                Console.WriteLine("Informe o dados do destinario: ");
                Console.Write("Digite o cpf: ");
                var cpfDestinario = Console.ReadLine().ToLower();
                var booleano = false;
                if (cpfDestinario == "sair") { menuPrincipal(); }
                if (cpfDestinario == cpf) { Console.WriteLine("\a\nErro você digitou o seu proprio CPF!"); Console.ReadKey(); continue; }
                if (cpfDestinario.Length == 11)
                {
                    try { var cpfDigitos = decimal.Parse(cpfDestinario); }
                    catch { Console.WriteLine("\a\nO CPF informado possui letras!"); Console.ReadKey(); continue; }
                    try
                    {
                        Console.Write("Digite o valor para transferencia: ");
                        var dinheiro = decimal.Parse(Console.ReadLine());
                        if (dinheiro > 0 & dinheiro <= 1000000)
                        {
                            if (dinheiro <= saldo)
                            {
                                switch (i)
                                {
                                    case 1:
                                        booleano = fb.prorpioBanco(cpfDestinario, dinheiro, cpf, 1);
                                        if (booleano) { Console.WriteLine($"\nDinheiro enviado no valor de {dinheiro}R$"); Console.ReadKey(); saldo -= dinheiro; }
                                        fb.criarComprovante(cpf, dinheiro, cpfDestinario);
                                        menuPrincipal();
                                        break;
                                    case 2:
                                        booleano = fb.prorpioBanco(cpfDestinario, dinheiro, cpf, 2);
                                        if (booleano) { Console.WriteLine($"\nDinheiro enviado no valor de {dinheiro}R$"); Console.ReadKey(); saldo -= dinheiro; }
                                        fb.criarComprovante(cpf, dinheiro, cpfDestinario);
                                        menuPrincipal();
                                        break;
                                }
                            }
                            else { Console.WriteLine("\a\nSaldo insuficiente para realizar a transferencia"); Console.ReadKey(); continue; }
                        }
                        else { Console.WriteLine("\a\nO valor permetido para transferencia é de 0.1R$ a 1.000.000R$"); Console.ReadKey(); }
                    }
                    catch { Console.WriteLine("\a\nFoi inserido palavras no valor da transferencia"); Console.ReadKey(); continue; }

                }
                else { Console.WriteLine("\a\nCPF quantidade de digitos inválido, tente novamnete!"); Console.ReadKey(); }
            }
        }
        //Metodo de fatura
        public void MenuFatura()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("NeoBank Sim - Menu Fatura");
                Console.WriteLine($"Fatura: {fatura:F2}");
                Console.WriteLine("1 - Ver historico");
                Console.WriteLine("2 - Pagar fatura");
                Console.WriteLine("X - Sair");
                Console.Write("Digite aqui: ");
                string opcao = Console.ReadLine();
                switch (opcao)
                {
                    case "1": nd(); break;
                    case "2": nd(); break;
                    case "x": menuPrincipal(); break;
                    default: Console.WriteLine("\a\nOpção inválida"); Console.ReadKey(); break;
                }
            }
        }
        //Historico de transações
        public void HistoricoTransacoes()
        {
            FuncoesBanco fb = new FuncoesBanco();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Historico de transações: ");
                fb.verHistorico(cpf);
                Console.Write("\nPressione Qualquer Tecla Para Continuar...");
                Console.ReadKey();
                menuPrincipal();
            }
        }
        //Metodo de pedir emprestismo
        public void MenuEmprestimo()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("NeoBank Sim - Menu Emprestimo");
                Console.WriteLine("1 - Pedir Emprestimo");
                Console.WriteLine("2 - Simular Emprestimo");
                Console.Write((emprestimo > 0 ? "3 - Pagar Emprestimo\n" : ""));
                Console.WriteLine("X - Sair");
                Console.Write("Digite aqui: ");
                var opcao = Console.ReadLine().ToLower();
                switch (opcao)
                {
                    case "1": if (emprestimo > 0) { Console.WriteLine("\a\nVocê ja solicitou um emprestimo!"); Console.ReadKey(); } else { pedirEmprestimo(1); } break;
                    case "2": pedirEmprestimo(2); break;
                    case "3": if (emprestimo > 0) { PagarEmprestimo(); } break;
                    case "x": menuPrincipal(); break;
                    default: Console.WriteLine("\a\nOpção inválida"); Console.ReadKey(); break;
                }
            }
        }
        //Metodo pedir emprestimo
        public void pedirEmprestimo(int i)
        {
            decimal juros = 0.02m;
            decimal valorPago = 0m;
            FuncoesBanco fb = new FuncoesBanco();
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("NeoBank Sim - Emprestimo\n");
                    Console.WriteLine("Digite 'sair' para voltar para o menu");
                    Console.Write("Insira o valor que deseja: ");
                    var valorstr = Console.ReadLine().ToLower();
                    if (valorstr == "sair") { menuPrincipal(); }
                    decimal valor = decimal.Parse(valorstr);
                    if (valor >= 1000 && valor <= 10000000)
                    {
                        Console.Write("Insira em quantos anos deseja pagar: ");
                        var anos = int.Parse(Console.ReadLine());
                        if (anos >= 1 && anos <= 30)
                        {
                            var novoJuros = (anos * juros) * 100;
                            var rt = valor * (100 + novoJuros);
                            valorPago = rt / 100;
                            Console.WriteLine("\nO Valor a ser pago é de: " + valorPago + "R$\n");
                            switch (i)
                            {
                                case 1:
                                    while (true)
                                    {
                                        Console.Write("Deseja confirmar o emprestimo[S||N]: ");
                                        string opcao = Console.ReadLine().ToLower();
                                        switch (opcao)
                                        {
                                            case "s":
                                                saldo += valor;
                                                emprestimo = valorPago;
                                                fb.SolicitarEmprestimo(cpf, valor, valorPago);
                                                menuPrincipal();
                                                break;
                                            case "n": menuPrincipal(); break;
                                            default: Console.WriteLine("\a\nOpção inválida"); Console.ReadKey(); Console.Clear(); break;
                                        }
                                    }
                                    break;
                                case 2: Console.ReadKey(); MenuEmprestimo(); break;
                            }
                        }
                        else { Console.WriteLine("A quantidade de anos permetido é de 1 a 30 anos!"); Console.ReadKey(); }
                    }
                    else { Console.WriteLine("\a\nO valor de emprestimo é de 1.000R$ a 10.000.000R$"); Console.ReadKey(); }
                }
                catch { Console.WriteLine("\a\nNo valor inserido possui letras!"); Console.ReadKey(); }

            }
        }
        //Metodo para pagar emprestimo
        public void PagarEmprestimo()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("NeoBank Sim - Pagar Emprestimo");
                    Console.WriteLine($"Saldo: {saldo:F2}R$");
                    Console.WriteLine($"Valor do emprestimo a ser pago: {emprestimo:F2}R$");
                    Console.WriteLine("Digite 'sair' para voltar para o menu");
                    Console.WriteLine("Informe o valor que deseja pagar: ");
                    Console.Write("Digite o valor: ");
                    var valorStr = Console.ReadLine().ToLower();
                    if (valorStr == "sair") { MenuEmprestimo(); }
                    var valor = decimal.Parse(valorStr);
                    if (valor <= 0) { Console.WriteLine("\a\nValor incorreto inserido"); Console.ReadKey(); }
                    else
                    {
                        if (valor > saldo) { Console.WriteLine("\aO valor inserido é maior que seu saldo atual"); Console.ReadKey(); }
                        else
                        {
                            if (valor > emprestimo) { Console.WriteLine("\a\nO valor é maior que o emprestimo solicitado!"); Console.ReadKey(); }
                            else
                            {
                                while (true)
                                {
                                    Console.Write("Deseja concluir pagamento[S||N]: ");
                                    string opcao = Console.ReadLine().ToLower();
                                    switch (opcao)
                                    {
                                        case "s":
                                            saldo -= valor;
                                            emprestimo -= valor;
                                            FuncoesBanco funcoesBanco = new FuncoesBanco();
                                            funcoesBanco.PagarEmprestimo(cpf, valor, valor);
                                            menuPrincipal();
                                            break;
                                        case "n": MenuEmprestimo(); break;
                                        default: break;
                                    }
                                }
                            }
                        }
                    }


                }
                catch { Console.WriteLine("\a\nNo valor inserido, possui letras"); Console.ReadKey(); }
            }
        }
        //Menu do Limite de cartão
        public void MenuLimite()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("NeoBank Sim - Menu De Limite");
                Console.Write((limite > 0 && cartao == 2 ? $"Limite Atual: {limite:F2}R$\n" : ""));
                Console.WriteLine("1 - Aumentar Limite");
                Console.WriteLine("2 - Diminuir Limite");
                Console.WriteLine("X - Sair");
                Console.Write("Digite aqui: ");
                var opcao = Console.ReadLine().ToLower();
                switch (opcao)
                {
                    case "1":
                        if (cartao == 0 || cartao == 1) { Console.WriteLine("\a\nVocê não possui cartão de credito, peça o seu!"); Console.ReadKey(); }
                        else { AumentarLimite(); }
                        break;
                    case "2":
                        if (cartao == 0 || cartao == 1) { Console.WriteLine("\a\nVocê não possui cartão de credito, peça o seu!"); Console.ReadKey(); }
                        else { DiminuirLimite(); }
                        break;
                    case "x": menuPrincipal(); break;
                    default: Console.WriteLine("\a\nOpção inválida"); Console.ReadKey(); break;
                }
            }
        }
        //Menu dos cartões
        public void MenuCartao()
        {
            var strCartao = "";
            switch (cartao)
            {
                case 0: strCartao = "Nenhum Cartão"; break;
                case 1: strCartao = "Cartão de Debito"; break;
                case 2: strCartao = "Cartão de Debito e Credito"; break;
            }
            while (true)
            {
                Console.Clear();
                Console.WriteLine("NeoBank Sim - Menu Cartão");
                Console.WriteLine($"Você possui: {strCartao}\n");
                Console.WriteLine("1 - Pedir cartão debito");
                Console.WriteLine("2 - Pedir cartão credito");
                Console.WriteLine("3 - Cancelar cartão");
                Console.WriteLine("4 - Consultar número do cartão");
                Console.WriteLine("X - Sair");
                Console.Write("Digite aqui: ");
                var opcao = Console.ReadLine().ToLower();
                switch (opcao)
                {
                    case "1":
                        if (cartao == 1 || cartao == 2) { Console.WriteLine("\a\nVocê já possui um cartão de debito"); Console.ReadKey(); }
                        else { CriarCartão(0); }
                        break;
                    case "2":
                        if (cartao == 0) { Console.WriteLine("\a\nVocê não possui um cartão"); Console.ReadKey(); }
                        if (cartao == 1) { CriarCartão(1); }
                        if (cartao == 2) { Console.WriteLine("\a\nVocê já possui um cartão de credito"); Console.ReadKey(); } 
                        break;
                    case "3":
                        if (cartao == 0) { Console.WriteLine("\a\nVocê não possui um cartão"); Console.ReadKey(); }
                        else { DeletarCartao(); }
                        break;
                    case "4":
                        if (cartao == 0) { Console.WriteLine("\a\nVocê não possui um cartão"); Console.ReadKey(); }
                        else { ConsultarNumeroCartao(); }
                        break;
                    case "x": menuPrincipal(); break;
                    default: Console.WriteLine("\a\nOpção inválida"); Console.ReadKey(); break;
                }
            }
        }
        //Gerar Numero para cartão
        public void CriarCartão(int i)
        {
            FuncoesBanco fb = new FuncoesBanco();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("NeoBank Sim - Solicitação de Cartão");
                Console.Write("Deseja confirmar sua solicitação de um novo cartão[S||N]: ");
                var opcao = Console.ReadLine().ToLower();
                switch (opcao)
                {
                    case "s":
                        switch (i)
                        {
                            case 0:  GerarCartao(); cartao +=  fb.SalvarCartão(cpf, numeroCartao); MenuCartao(); break;
                            case 1: cartao += fb.SalvarCartão(cpf, numeroCartao);MenuCartao();  break;
                        }
                        break;
                    case "n": MenuCartao(); break;
                    default: Console.WriteLine("\a\nOpção inválida"); Console.ReadKey(); break;
                }
            }
        }
        //Gerar os numeros
        public void GerarCartao()
        {
            Random random = new Random();
            for (int i = 0; i < 12; i++)
            {
                numeroCartao += random.Next(0,10);
            }
        }
        //Consultar o numero do cartão
        public void ConsultarNumeroCartao()
        {
            FuncoesBanco fb = new FuncoesBanco();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("NeoBank Sim - Consultar Cartão");
                Console.WriteLine("Digite 'sair' para voltar para o menu");
                Console.WriteLine("Informe sua senha abaixo: \n");
                Console.Write("Digite sua senha: ");
                var opcao = Console.ReadLine().ToLower();
                if (opcao == "sair") { MenuCartao(); }
                else
                {
                    var booleano = fb.ExibirCartao(cpf, opcao, numeroCartao);
                    if (booleano) { menuPrincipal(); }
                    else { }
                }
            }
        }
        //Deletar cartão
        public void DeletarCartao()
        {
            FuncoesBanco fb = new FuncoesBanco();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("NeoBank Sim - Deletar cartão");
                Console.Write("Você deseja excluir seu cartão[S||N]: ");
                var opcao = Console.ReadLine().ToLower();
                switch (opcao)
                {
                    case "s":
                        if(fatura > 0) { Console.WriteLine("\a\nNão é possivel deletar cartão, a sua fatura está em aberto ainda"); Console.ReadKey(); }
                        else { fb.DeletarCartao(cpf); cartao = 0; numeroCartao = "0"; MenuCartao(); }
                            break;
                    case "n": MenuCartao(); break;
                    default: Console.WriteLine("\a\nOpção inválida"); Console.ReadKey(); break;
                }
            }
        }

        //Menu das contas
        public void MenuConta()
        {
            while (true) 
            {
                Console.Clear();
                Console.WriteLine("NeoBank Sim - Menu Conta");
                Console.WriteLine("1 - Alterar senha");
                Console.WriteLine("2 - Trocar de conta");
                Console.WriteLine("X - Sair");
                Console.Write("Digite aqui: ");
                var opcao = Console.ReadLine().ToLower();
                switch (opcao)
                {
                    case "1": MudarSenha(); break;
                    case "2": Program p = new Program(); p.MenuInicial(); break;
                    case "x": menuPrincipal(); break;
                    default: Console.WriteLine("\a\nOpção inválida!"); Console.ReadKey(); break;
                }
            }
        }
        //Menu da aumento  de limite
        public void AumentarLimite()
        {
            FuncoesBanco fb = new FuncoesBanco();
            while(true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("NeoBank Sim - Menu de limite");
                    Console.WriteLine("Digite 'sair' para voltar para o menu");
                    Console.WriteLine("Informe abaixo o valor do seu novo limite\n");
                    Console.Write("Digite o valor: ");
                    string valorStr = Console.ReadLine().ToLower();
                    if(valorStr == "sair") { MenuLimite(); }
                    var valor = decimal.Parse(valorStr);
                    if (valor <= limite) { Console.WriteLine("\a\nO valor tem que ser maior que seu limite atual"); Console.ReadKey(); }
                    else
                    {
                        if (valor > limite && valor < 10000)
                        {
                            limite = uint.Parse(valorStr);
                            fb.AumentarLimite(cpf, limite, 1);
                            MenuLimite();
                        }
                        else { Console.WriteLine("\a\nO valor do novo limite tem que ser ate 10.000R$ "); Console.ReadKey(); }
                    }
                    
                }
                catch { Console.WriteLine("\a\nNo valor inserido possui letras"); Console.ReadKey(); }
            }
        }
        //Menu da aumento  de limite
        public void DiminuirLimite()
        {
            FuncoesBanco fb = new FuncoesBanco();
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("NeoBank Sim - Menu de limite");
                    Console.WriteLine("Digite 'sair' para voltar para o menu");
                    Console.WriteLine("Informe abaixo o valor do seu novo limite\n");
                    Console.Write("Digite o valor: ");
                    string valorStr = Console.ReadLine().ToLower();
                    if (valorStr == "sair") { MenuLimite(); }
                    var valor = decimal.Parse(valorStr);
                    if (valor >= limite) { Console.WriteLine("\a\nO valor tem que ser menor que seu limite atual"); Console.ReadKey(); }
                    else
                    {
                        if (valor > 10 && valor < limite)
                        {
                            limite = uint.Parse(valorStr);
                            fb.AumentarLimite(cpf, limite, 2);
                            MenuLimite();
                        }
                        else { Console.WriteLine("\a\nO valor do novo limite tem que ser ate 10$ "); Console.ReadKey(); }
                    }
                    
                }
                catch { Console.WriteLine("\a\nNo valor inserido possui letras"); Console.ReadKey(); }
            }
        }
        //Nova senha
        public void MudarSenha()
        {
            FuncoesBanco fb = new FuncoesBanco();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("NeoBank Sim - Mudar senha");
                Console.WriteLine("Digite 'sair' para voltar para o menu");
                Console.WriteLine("Informe abaixo sobre sua senha:\n ");
                Console.Write("Digite sua senha atual: ");
                string senhaAntiga = Console.ReadLine();
                if (senhaAntiga == "sair") { menuPrincipal(); }
                var booleano = fb.PesquisarUsuario(cpf, senhaAntiga);
                if (booleano)
                {
                    while (true)
                    {
                        Console.Write("Digite sua nova senha: ");
                        string senhaNova = Console.ReadLine();
                        if(senhaNova.Length >= 6 && senhaNova.Length <= 30)
                        {
                            fb.MudarSenha(cpf, senhaNova); menuPrincipal();
                        }
                        else { Console.WriteLine("\a\nA senha deve possuir de 6 a 30 caracteres!"); Console.ReadKey(); Console.Clear(); }
                    }
                }
            }
        }
    }
}