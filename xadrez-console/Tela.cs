using System;
using tabuleiro;
using tabuleiro.Enums;
using xadrez;
using System.Collections.Generic;
using tabuleiro.Exceptions;

namespace xadrez_console {
    class Tela {

        public static void ImprimirPartida(PartidaXadrez partida) {
            ImprimirTabuleiro(partida.Tab);
            Console.WriteLine();
            ImprimirPecasCapturadas(partida);
            Console.WriteLine();
            Console.WriteLine("Turno: " + partida.Turno);
            if (!partida.Terminada) {
                Console.Write("Aguardando jogada: ");
                Console.ForegroundColor = (partida.JogadorAtual == Cor.Preta) ? ConsoleColor.Green : ConsoleColor.White;
                Console.Write(partida.JogadorAtual);
                Console.ForegroundColor = ConsoleColor.White;
                if (partida.Xeque) {
                    Console.WriteLine();
                    Console.Write("VOCÊ ESTÁ EM XEQUE!");
                }
            } else {
                Console.WriteLine("XEQUE MATE!");
                Console.WriteLine("Vencedor: " + partida.JogadorAtual);
            }
        }

        public static void ImprimirPartida(PartidaXadrez partida, bool[,] posicoesPossiveis) {
            ImprimirTabuleiro(partida.Tab, posicoesPossiveis);
            Console.WriteLine();
            ImprimirPecasCapturadas(partida);
            Console.WriteLine();
            Console.WriteLine("Turno: " + partida.Turno);
            Console.Write("Aguardando jogada: ");
            Console.ForegroundColor = (partida.JogadorAtual == Cor.Preta) ? ConsoleColor.Green : ConsoleColor.White;
            Console.Write(partida.JogadorAtual);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ImprimirPecasCapturadas(PartidaXadrez partida) {
            Console.WriteLine("Peças capturadas:");
            Console.Write("Brancas: ");
            ImprimirConjunto(partida.PecasCapturadas(Cor.Branca));
            Console.WriteLine();
            Console.Write("Pretas: ");
            ImprimirConjunto(partida.PecasCapturadas(Cor.Preta));
            Console.WriteLine();
        }

        public static void ImprimirConjunto(HashSet<Peca> conjunto) {
            Console.Write("[");
            foreach (Peca p in conjunto) {
                Console.ForegroundColor = (p.Cor == Cor.Preta) ? ConsoleColor.Green : ConsoleColor.White;
                Console.Write($"{p} ");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("]");
        }

        public static void ImprimirTabuleiro(Tabuleiro tab) {
            for (int i = 0; i < tab.Linhas; i++) {
                Console.Write(8 - i);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(" # ");
                Console.ForegroundColor = ConsoleColor.White;
                for (int j = 0; j < tab.Colunas; j++) {
                    ImprimirPeca(tab.Peca(i, j));
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    # # # # # # # # ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("    A B C D E F G H ");
        }

        internal static PosicaoXadrez LerPosicaoXadrez() {
            char coluna;
            int linha;
            try {
                string pos = Console.ReadLine();
                coluna = pos[0];
                linha = int.Parse(pos[1] + "");
            } catch (FormatException) {
                throw new TabuleiroException("Informe a posição no padrão ColunaLinha. Ex.: a1");

            }
            return new PosicaoXadrez(coluna, linha);
        }

        public static void ImprimirPeca(Peca peca) {
            if (peca == null) {
                Console.Write("- ");
            } else {
                if (peca.Cor == tabuleiro.Enums.Cor.Branca) {
                    Console.Write(peca);
                }
                if (peca.Cor == tabuleiro.Enums.Cor.Preta) {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
        }

        internal static void ImprimirTabuleiro(Tabuleiro tab, bool[,] posicoesPossiveis) {
            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkGray;

            for (int i = 0; i < tab.Linhas; i++) {
                Console.Write(8 - i);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(" # ");
                Console.ForegroundColor = ConsoleColor.White;
                for (int j = 0; j < tab.Colunas; j++) {
                    if (posicoesPossiveis[i, j]) {
                        Console.BackgroundColor = fundoAlterado;
                    } else {
                        Console.BackgroundColor = fundoOriginal;
                    }
                    ImprimirPeca(tab.Peca(i, j));
                    Console.BackgroundColor = fundoOriginal;
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    # # # # # # # # ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("    A B C D E F G H ");
            Console.BackgroundColor = fundoOriginal;
        }
    }
}
