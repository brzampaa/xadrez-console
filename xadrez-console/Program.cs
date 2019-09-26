using System;
using tabuleiro;
using xadrez;
using tabuleiro.Enums;

namespace xadrez_console {
    class Program {
        static void Main(string[] args) {

            PosicaoXadrez pos = new PosicaoXadrez('c', 7);
            Console.WriteLine(pos);

            Console.WriteLine(pos.ToPosicao());
            
            /*try {

                Tabuleiro tab = new Tabuleiro(8, 8);
                tab.ColocarPeca(new Torre(Cor.Preta, tab), new Posicao(0, 0));
                tab.ColocarPeca(new Torre(Cor.Preta, tab), new Posicao(1, 3));
                tab.ColocarPeca(new Rei(Cor.Preta, tab), new Posicao(0, 2));

                Tela.ImprimirTabuleiro(tab);
            } catch (Exception e) {

                Console.WriteLine(e.Message);
            }*/
            
        }
    }
}
