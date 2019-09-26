using System;
using tabuleiro;
using tabuleiro.Enums;
using xadrez;

namespace xadrez_console {
    class Tela {

        public static void ImprimirTabuleiro(Tabuleiro tab) {
            for (int i = 0; i < tab.Linhas; i++) {
                Console.Write(8 - i + " ");
                for (int j = 0; j < tab.Colunas; j++) {
                    if (tab.Peca(i, j) == null) {
                        Console.Write("- ");
                    } else {
                        ImprimirPeca(tab.Peca(i, j));
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("  A B C D E F G H ");
        }

        internal static PosicaoXadrez LerPosicaoXadrez() {
            string pos = Console.ReadLine();
            char coluna = pos[0];
            int linha = int.Parse(pos[1]+"");
            return new PosicaoXadrez(coluna, linha);
        }

        public static void ImprimirPeca(Peca peca) {
            if (peca.Cor == tabuleiro.Enums.Cor.Branca) {
                Console.Write(peca);
            }
            if (peca.Cor == tabuleiro.Enums.Cor.Preta) {
                ConsoleColor aux = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(peca);
                Console.ForegroundColor = aux;
            }
        }
    }
}
