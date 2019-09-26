using System;
using tabuleiro;
using xadrez;
using tabuleiro.Enums;

namespace xadrez_console {
    class Program {
        static void Main(string[] args) {

            /*PosicaoXadrez pos = new PosicaoXadrez('c', 7);
            Console.WriteLine(pos);

            Console.WriteLine(pos.ToPosicao());*/
            
            try {

                PartidaXadrez partida = new PartidaXadrez();
                while (!partida.Terminada) {
                    Console.Clear();
                    Tela.ImprimirTabuleiro(partida.Tab);

                    Console.WriteLine();
                    Console.Write("Origem: ");
                    Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();
                    Console.Write("Destino: ");
                    Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();

                    partida.ExecutaMovimento(origem, destino);
                }
            } catch (Exception e) {

                Console.WriteLine(e.Message);
            }
            
        }
    }
}
