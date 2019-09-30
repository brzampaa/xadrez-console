﻿using System;
using tabuleiro;
using xadrez;
using tabuleiro.Enums;
using tabuleiro.Exceptions;

namespace xadrez_console {
    class Program {
        static void Main(string[] args) {

            /*PosicaoXadrez pos = new PosicaoXadrez('c', 7);
            Console.WriteLine(pos);

            Console.WriteLine(pos.ToPosicao());*/

            try {

                PartidaXadrez partida = new PartidaXadrez();
                while (!partida.Terminada) {
                    try {
                        Console.Clear();
                        Tela.ImprimirPartida(partida);

                        Console.ForegroundColor = (partida.JogadorAtual == Cor.Preta) ? ConsoleColor.Green : ConsoleColor.White;

                        Console.Write(partida.JogadorAtual);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.Write("Origem: ");
                        Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();
                        partida.ValidarPosicaoOrigem(origem);

                        bool[,] posicoesPossiveis = partida.Tab.Peca(origem).MovimentosPossiveis();

                        Console.Clear();
                        Tela.ImprimirPartida(partida, posicoesPossiveis);

                        Console.ForegroundColor = (partida.JogadorAtual == Cor.Preta) ? ConsoleColor.Green : ConsoleColor.White;
                        
                        Console.Write(partida.JogadorAtual);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.Write("Destino: ");
                        Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();
                        partida.ValidarPosicaoDestino(origem, destino);

                        partida.RealizaJogada(origem, destino);
                    } catch (TabuleiroException e) {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

        }
    }
}
