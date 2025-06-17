using System;
using System.Collections.Generic;
using System.IO;

    public class NoAVL<T> where T : IComparable<T>
    {
        public T Valor { get; set; }
        public NoAVL<T> Esquerda { get; set; }
        public NoAVL<T> Direita { get; set; }
        public int Altura { get; set; }

        public NoAVL(T valor)
        {
            Valor = valor;
            Altura = 1;
        }
    }

    public class ArvoreAVL<T> where T : IComparable<T>
    {
        private NoAVL<T> raiz;

        private int Altura(NoAVL<T> no)
        {
            return no == null ? 0 : no.Altura;
        }

        private int FatorBalanceamento(NoAVL<T> no)
        {
            if (no == null)
                return 0;
            return Altura(no.Esquerda) - Altura(no.Direita);
        }

        private void AtualizarAltura(NoAVL<T> no)
        {
            if (no != null)
            {
                no.Altura = Math.Max(Altura(no.Esquerda), Altura(no.Direita)) + 1;
            }
        }

        private NoAVL<T> RotacaoDireita(NoAVL<T> y)
        {
            var x = y.Esquerda;
            var T2 = x.Direita;

            x.Direita = y;
            y.Esquerda = T2;

            AtualizarAltura(y);
            AtualizarAltura(x);

            return x;
        }

        private NoAVL<T> RotacaoEsquerda(NoAVL<T> x)
        {
            var y = x.Direita;
            var T2 = y.Esquerda;

            y.Esquerda = x;
            x.Direita = T2;

            AtualizarAltura(x);
            AtualizarAltura(y);

            return y;
        }

        public void Inserir(T valor)
        {
            raiz = Inserir(raiz, valor);
        }

        private NoAVL<T> Inserir(NoAVL<T> no, T valor)
        {
            if (no == null)
                return new NoAVL<T>(valor);

            if (valor.CompareTo(no.Valor) < 0)
                no.Esquerda = Inserir(no.Esquerda, valor);
            else if (valor.CompareTo(no.Valor) > 0)
                no.Direita = Inserir(no.Direita, valor);
            else
                return no;

            AtualizarAltura(no);

            int balance = FatorBalanceamento(no);

            if (balance > 1 && valor.CompareTo(no.Esquerda.Valor) < 0)
                return RotacaoDireita(no);

            if (balance < -1 && valor.CompareTo(no.Direita.Valor) > 0)
                return RotacaoEsquerda(no);

            if (balance > 1 && valor.CompareTo(no.Esquerda.Valor) > 0)
            {
                no.Esquerda = RotacaoEsquerda(no.Esquerda);
                return RotacaoDireita(no);
            }

            if (balance < -1 && valor.CompareTo(no.Direita.Valor) < 0)
            {
                no.Direita = RotacaoDireita(no.Direita);
                return RotacaoEsquerda(no);
            }

            return no;
        }

        public void Remover(T valor)
        {
            raiz = Remover(raiz, valor);
        }

        private NoAVL<T> Remover(NoAVL<T> no, T valor)
        {
            if (no == null)
                return no;

            if (valor.CompareTo(no.Valor) < 0)
                no.Esquerda = Remover(no.Esquerda, valor);
            else if (valor.CompareTo(no.Valor) > 0)
                no.Direita = Remover(no.Direita, valor);
            else
            {
                if (no.Esquerda == null || no.Direita == null)
                {
                    NoAVL<T> temp = no.Esquerda ?? no.Direita;

                    if (temp == null)
                    {
                        temp = no;
                        no = null;
                    }
                    else
                        no = temp;
                }
                else
                {
                    var temp = MinimoValorNo(no.Direita);
                    no.Valor = temp.Valor;
                    no.Direita = Remover(no.Direita, temp.Valor);
                }
            }

            if (no == null)
                return no;

            AtualizarAltura(no);

            int balance = FatorBalanceamento(no);

            if (balance > 1 && FatorBalanceamento(no.Esquerda) >= 0)
                return RotacaoDireita(no);

            if (balance > 1 && FatorBalanceamento(no.Esquerda) < 0)
            {
                no.Esquerda = RotacaoEsquerda(no.Esquerda);
                return RotacaoDireita(no);
            }

            if (balance < -1 && FatorBalanceamento(no.Direita) <= 0)
                return RotacaoEsquerda(no);

            if (balance < -1 && FatorBalanceamento(no.Direita) > 0)
            {
                no.Direita = RotacaoDireita(no.Direita);
                return RotacaoEsquerda(no);
            }

            return no;
        }

        private NoAVL<T> MinimoValorNo(NoAVL<T> no)
        {
            NoAVL<T> atual = no;
            while (atual.Esquerda != null)
                atual = atual.Esquerda;
            return atual;
        }

        public bool Buscar(T valor)
        {
            return Buscar(raiz, valor);
        }

        private bool Buscar(NoAVL<T> no, T valor)
        {
            if (no == null)
                return false;

            if (valor.CompareTo(no.Valor) < 0)
                return Buscar(no.Esquerda, valor);
            else if (valor.CompareTo(no.Valor) > 0)
                return Buscar(no.Direita, valor);
            else
                return true;
        }

        public List<T> PreOrdem()
        {
            var resultado = new List<T>();
            PreOrdem(raiz, resultado);
            return resultado;
        }

        private void PreOrdem(NoAVL<T> no, List<T> resultado)
        {
            if (no != null)
            {
                resultado.Add(no.Valor);
                PreOrdem(no.Esquerda, resultado);
                PreOrdem(no.Direita, resultado);
            }
        }

        public Dictionary<T, int> ObterFatoresBalanceamento()
        {
            var fatores = new Dictionary<T, int>();
            ObterFatoresBalanceamento(raiz, fatores);
            return fatores;
        }

        private void ObterFatoresBalanceamento(NoAVL<T> no, Dictionary<T, int> fatores)
        {
            if (no != null)
            {
                fatores[no.Valor] = FatorBalanceamento(no);
                ObterFatoresBalanceamento(no.Esquerda, fatores);
                ObterFatoresBalanceamento(no.Direita, fatores);
            }
        }

        public int AlturaArvore()
        {
            return Altura(raiz);
        }
    }


    public class ProcessadorComandosAVL
    {
        public static void ProcessarArquivo(string caminhoArquivo)
        {
            var arvore = new ArvoreAVL<int>();

            foreach (var linha in File.ReadLines(caminhoArquivo))
            {
                var partes = linha.Trim().Split(' ');
                if (partes.Length == 0) continue;

                var comando = partes[0].ToUpper();

                try
                {
                    switch (comando)
                    {
                        case "I":
                            if (partes.Length < 2) throw new ArgumentException("Comando I requer um valor");
                            int valorInserir = int.Parse(partes[1]);
                            arvore.Inserir(valorInserir);
                            break;

                        case "R":
                            if (partes.Length < 2) throw new ArgumentException("Comando R requer um valor");
                            int valorRemover = int.Parse(partes[1]);
                            arvore.Remover(valorRemover);
                            break;

                        case "B":
                            if (partes.Length < 2) throw new ArgumentException("Comando B requer um valor");
                            int valorBuscar = int.Parse(partes[1]);
                            bool encontrado = arvore.Buscar(valorBuscar);
                            Console.WriteLine(encontrado ? "Valor encontrado" : "Valor nao encontrado");
                            break;

                        case "P":
                            var preOrdem = arvore.PreOrdem();
                            Console.WriteLine("Arvore em pre-ordem: " + string.Join(" ", preOrdem));
                            break;

                        case "F":
                            var fatores = arvore.ObterFatoresBalanceamento();
                            Console.WriteLine("Fatores de balanceamento:");
                            foreach (var kvp in fatores)
                            {
                                Console.WriteLine($"No {kvp.Key}: Fator de balanceamento {kvp.Value}");
                            }
                            break;

                        case "H":
                            Console.WriteLine($"Altura da arvore: {arvore.AlturaArvore()}");
                            break;

                        default:
                            Console.WriteLine($"Comando desconhecido: {comando}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar comando '{linha}': {ex.Message}");
                }
            }
        }
    }

    class Program
    {
        static void Main()
        {
            string nomeArquivo = "entrada.txt";
            string caminhoArquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nomeArquivo);

            if (!File.Exists(caminhoArquivo))
            {
                Console.WriteLine($"Arquivo '{nomeArquivo}' não encontrado na pasta do executável.");
                Console.WriteLine($"Procurando em: {caminhoArquivo}");
                return;
            }

            try
            {
                Console.WriteLine($"Processando arquivo: {caminhoArquivo}");
                ProcessadorComandosAVL.ProcessarArquivo(caminhoArquivo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar arquivo: {ex.Message}");
            }

            Console.WriteLine("\nPressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }