using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _18181_18185_PojetoIIED
{
    public partial class Form1 : Form
    {
        private string[] ops = new string[] { "+", "-", "/", "*", "^", "(", ")" }; 

        public Form1()
        {
            InitializeComponent();
        }

        private void btnExp_Click(object sender, EventArgs e)
        {
            lblExpres.Text = ""; //limpa a label da expressão
            Button clicado = (Button)sender; //envia o objeto
            txtVisor.Text += clicado.Text; //atualiza o visor com o tecla da tecla clicada 
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lblExpres.Text = ""; //limpa a label da expressão
            txtVisor.Text = ""; //limpa o visor da expressão 
            txtResultado.Text = ""; //limpa o visor do resultado 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblExpres.Text = ""; //limpa a label da expressão
        }

        private void btnIgual_Click(object sender, EventArgs e)
        {
            FilaLista<string> infixa = new FilaLista<string>();
            FilaLista<string> posfixa = new FilaLista<string>();
            PilhaLista<string> ops = new PilhaLista<string>(); //declara e instancia as 3 listas
            string expressao = txtVisor.Text; //expressão a ser calculada

            for (int i = 0; i < expressao.Length; i++) //for até o fim da expressão 
            {
                string elemento = ""; 

                if (!IsOp(expressao[i].ToString())) //se não for uma operção  
                {
                    elemento = "";
                    int inicial = i; 
                    while (inicial + elemento.Length < expressao.Length && (!IsOp(expressao[inicial + elemento.Length].ToString()) || expressao[inicial + elemento.Length] == '.')) //enquanto não acabou a expressão, não for operação ou não for ponto 
                        elemento += expressao[inicial + elemento.Length]; //incrementa o elemento com o valor da posição específic da expressão 

                    i = inicial + elemento.Length - 1;
                    posfixa.Enfileirar(elemento); //enfileira na posfixa o elemento
                }
                else //se for operação 
                {
                    elemento = expressao[i] + "";

                    while (!ops.EstaVazia() && TemPrecedencia(ops.OTopo()[0], elemento[0])) //enquanto não estiver vazia e tiver precedência 
                    {
                        char op = ops.OTopo()[0]; 
                        if (op == '(')
                            break;
                        else //se não for abre parenteses 
                        {
                            posfixa.Enfileirar(op + ""); //enfileira 
                            ops.Desempilhar(); //desempilha 
                        }
                    }

                    if (elemento != ")") //se não for fecha parenteses 
                        ops.Empilhar(elemento); //empilha
                    else //se for
                        ops.Desempilhar(); //desempilha
                }
                if (elemento != "(" && elemento != ")") //se não for parenteses
                    infixa.Enfileirar(elemento); //enfileira o elemento 
            }

            while (!ops.EstaVazia()) //enquanto não estiver vazia
            {
                string op = ops.Desempilhar(); //pega o primeiro da pilha

                if (op != "(" && op != ")") //se não for parenteses
                {
                    posfixa.Enfileirar(op); //enfileira 
                }
            }

            escreverSeq(infixa, posfixa); //escreve na tela as sequências
            txtResultado.Text = CalcularResultado(posfixa).ToString(); //calcula e escreve o resultado 
        }

        private void escreverSeq(FilaLista<string> inf, FilaLista<string> pos)
        {   
            char letra = 'A'; 
            string[] vet = pos.ToArray(); //lista de pósfixa
            lblExpres.Text += "Posfixa: ";

            for (int i = 0; i < vet.Length; i++) //enquanto não acabou o vetor
            {
                if (IsOp(vet[i])) //se for operação 
                {
                    lblExpres.Text += vet[i]; //armazena na string 
                }
                else //se não for operação 
                lblExpres.Text += letra++; //armazena a letra e incrementa 
            }

            lblExpres.Text += "\n" + "Infixa: "; 
            letra = 'A';
            vet = inf.ToArray(); //lista de infixa

            for (int i = 0; i < vet.Length; i++) //enquanto não acabou o vetor
            {
                if (IsOp(vet[i])) //se for operação 
                {
                    lblExpres.Text += vet[i]; //armazena na string 
                }
                else //se não for operação 
                    lblExpres.Text += letra++; //armazena a letra e incrementa 
            }
        }

        private double CalcularResultado(FilaLista<string> expre)
        {
            PilhaLista<double> valores = new PilhaLista<double>();
            double v1 = 0, v2 = 0, result = 0;
            string[] vet = expre.ToArray();

            for (int c = 0; c < vet.Length; c++) //for até o fim do vetor
            {
                if (!IsOp(vet[c])) //se não for operação
                    valores.Empilhar(double.Parse(vet[c].Replace('.', ','))); //empilha e troca ponto por vírgula
                else
                {
                    v1 = valores.Desempilhar(); //desempilha o 1º valor
                    v2 = valores.Desempilhar(); //desempilha o 2º valor

                    switch (vet[c]) //switch do valor caso seja operção 
                    {
                        case "+": result = v2 + v1; break; //se for + soma
                        case "-": result = v2 - v1; break; //se for - subtrai
                        case "*": result = v2 * v1; break; //se for * multiplica
                        case "/": //se for divisão 
                            if (v1 == 0)
                                throw new DivideByZeroException("Divisão por 0"); //se for 0 joga exceção 
                            result = v2 / v1; break; //se não for zero calcula
                        case "^": result = Math.Pow(v2, v1); break; //se for ^ faz potência 
                    }
                    valores.Empilhar(result); //empilha o resultado
                }
            }

            return valores.Desempilhar(); //no fim desempilha o resultado final
        }

        private bool IsOp (string c)
        {
            return ops.Contains(c); //retorna true ou false, dependendo se há o valor 'c' no vetor de string de operações
        }

        private bool TemPrecedencia(char topo, char operacao)
        {
            switch (topo) 
            {
                case '+':
                case '-':
                    if (operacao == '+' || operacao == '-' || operacao == ')')
                        return true;
                    break;

                case '*':
                case '/':
                case '^':
                    if (operacao == '+' || operacao == '-' || operacao == '*' || operacao == '/' || operacao == ')')
                        return true;
                    break;

                case '(':
                    if (operacao == ')')
                        return true;
                    break;

            }
            return false;
        }
    }  
}
