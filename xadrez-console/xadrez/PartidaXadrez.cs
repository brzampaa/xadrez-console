﻿using System;
using tabuleiro;
using tabuleiro.Enums;
using tabuleiro.Exceptions;
using xadrez;
using System.Collections.Generic;

namespace xadrez {
    class PartidaXadrez {
        public Tabuleiro Tab { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }
        public bool Xeque { get; private set; } = false;

        public Peca VulneravelEnPassant { get; private set; }

        private HashSet<Peca> Pecas;
        private HashSet<Peca> Capturadas;

        public PartidaXadrez() {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            Xeque = false;
            VulneravelEnPassant = null;
            Pecas = new HashSet<Peca>();
            Capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino) {
            Peca p = Tab.RetirarPeca(origem);
            p.IncrementarQteMovimentos();
            Peca pecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);
            if (pecaCapturada != null) {
                Capturadas.Add(pecaCapturada);
            }

            //Jogada Especial Roque Pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2) {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tab.RetirarPeca(origemT);
                T.IncrementarQteMovimentos();
                Tab.ColocarPeca(T, destinoT);
                Console.WriteLine("\nA jogada especial ROQUE será efetuada!\nPressione enter para continuar.");
                Console.ReadLine();
            }

            //Jogada Especial Roque Pequeno
            if (p is Rei && destino.Coluna == origem.Coluna - 2) {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.RetirarPeca(origemT);
                T.IncrementarQteMovimentos();
                Tab.ColocarPeca(T, destinoT);
                Console.WriteLine("\nA jogada especial ROQUE será efetuada!\nPressione enter para continuar.");
                Console.ReadLine();
            }

            //Jogada Especial En Passant
            if (p is Peao) {
                if (origem.Coluna != destino.Coluna && pecaCapturada == null) {
                    Posicao posPeao;
                    if (p.Cor == Cor.Branca) {
                        posPeao = new Posicao(destino.Linha + 1, destino.Coluna);
                    } else {
                        posPeao = new Posicao(destino.Linha - 1, destino.Coluna);
                    }
                    pecaCapturada = Tab.RetirarPeca(posPeao);
                    Capturadas.Add(pecaCapturada);
                    Console.WriteLine("\nA jogada especial EN PASSANT será efetuada!\nPressione enter para continuar.");
                    Console.ReadLine();
                }
            }

            return pecaCapturada;
        }

        public void RealizaJogada(Posicao origem, Posicao destino) {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);
            if (EstaEmXeque(JogadorAtual)) {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            Peca p = Tab.Peca(destino);

            //Jogada Especial: Promoção
            if (p is Peao) {
                if ((p.Cor == Cor.Branca && destino.Linha == 0) || (p.Cor == Cor.Preta && destino.Linha == 7)) {
                    p = Tab.RetirarPeca(destino);
                    Pecas.Remove(p);
                    Peca dama = new Dama(p.Cor, Tab);
                    Tab.ColocarPeca(dama, destino);
                    Pecas.Add(dama);
                    Console.WriteLine("\nSeu peão será promovido à DAMA!\nPressione enter para continuar.");
                    Console.ReadLine();
                }
            }

            Xeque = (EstaEmXeque(Adversario(JogadorAtual))) ? true : false;

            if (TesteXequeMate(Adversario(JogadorAtual))) {
                Terminada = true;
            } else {
                Turno++;
                MudaJogador();
            }

            

            //Jogada Especial: En Passant
            if (p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2)) {
                VulneravelEnPassant = p;
            } else {
                VulneravelEnPassant = null;
            }

        }

        private void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada) {
            Peca p = Tab.RetirarPeca(destino);
            p.DecrementarQteMovimentos();
            if (pecaCapturada != null) {
                Tab.ColocarPeca(pecaCapturada, destino);
                Capturadas.Remove(pecaCapturada);
            }
            Tab.ColocarPeca(p, origem);

            //Jogada Especial Roque Pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2) {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tab.RetirarPeca(destinoT);
                T.DecrementarQteMovimentos();
                Tab.ColocarPeca(T, origemT);
            }

            //Jogada Especial Roque Pequeno
            if (p is Rei && destino.Coluna == origem.Coluna - 2) {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.RetirarPeca(destinoT);
                T.DecrementarQteMovimentos();
                Tab.ColocarPeca(T, origemT);
            }

            //Jogada Especial En Passant
            if (p is Peao) {
                if (origem.Coluna != destino.Coluna && pecaCapturada == VulneravelEnPassant) {
                    Peca peao = Tab.RetirarPeca(destino);
                    Posicao posPeao;
                    if (p.Cor == Cor.Branca) {
                        posPeao = new Posicao(3, destino.Coluna);
                    } else {
                        posPeao = new Posicao(4, destino.Coluna);
                    }
                    Tab.ColocarPeca(peao, posPeao);
                    Capturadas.Remove(pecaCapturada);
                }
            }
        }

        public void ValidarPosicaoOrigem(Posicao pos) {
            try {
                Tab.Peca(pos);
            } catch (IndexOutOfRangeException) {
                throw new TabuleiroException("A posição informada não existe!");
            }
            if (Tab.Peca(pos) == null) {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }
            if (JogadorAtual != Tab.Peca(pos).Cor) {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!Tab.Peca(pos).ExisteMovimentosPossiveis()) {
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void ValidarPosicaoDestino(Posicao origem, Posicao destino) {
            if (!Tab.Peca(origem).MovimentoPossivel(destino)) {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }

        private void MudaJogador() {
            JogadorAtual = (JogadorAtual == Cor.Branca) ? Cor.Preta : Cor.Branca;
        }

        public HashSet<Peca> PecasCapturadas(Cor cor) {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca p in Capturadas) {
                if (p.Cor == cor) {
                    aux.Add(p);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor cor) {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca p in Pecas) {
                if (p.Cor == cor) {
                    aux.Add(p);
                }
            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }

        private Cor Adversario(Cor cor) {
            return (cor == Cor.Branca) ? Cor.Preta : Cor.Branca;
        }

        private Peca GetRei(Cor cor) {
            foreach (Peca p in PecasEmJogo(cor)) {
                if (p is Rei) {
                    return p;
                }
            }
            return null;
        }

        public bool EstaEmXeque(Cor cor) {
            Peca reiAdversario = GetRei(cor);
            foreach (Peca p in PecasEmJogo(Adversario(cor))) {
                bool[,] mat = p.MovimentosPossiveis();
                if (mat[reiAdversario.Posicao.Linha, reiAdversario.Posicao.Coluna]) {
                    return true;
                }
            }
            return false;
        }

        public bool TesteXequeMate(Cor cor) {
            if (!EstaEmXeque(cor)) {
                return false;
            }
            foreach (Peca p in PecasEmJogo(cor)) {
                bool[,] mat = p.MovimentosPossiveis();
                for (int i = 0; i < Tab.Linhas; i++) {
                    for (int j = 0; j < Tab.Colunas; j++) {
                        if (mat[i, j]) {
                            Posicao origem = p.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque) {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca) {
            Tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            Pecas.Add(peca);
        }

        private void ColocarPecas() {
            ColocarNovaPeca('a', 1, new Torre(Cor.Branca, Tab));
            ColocarNovaPeca('b', 1, new Cavalo(Cor.Branca, Tab));
            ColocarNovaPeca('c', 1, new Bispo(Cor.Branca, Tab));
            ColocarNovaPeca('d', 1, new Dama(Cor.Branca, Tab));
            ColocarNovaPeca('e', 1, new Rei(Cor.Branca, Tab, this));
            ColocarNovaPeca('f', 1, new Bispo(Cor.Branca, Tab));
            ColocarNovaPeca('g', 1, new Cavalo(Cor.Branca, Tab));
            ColocarNovaPeca('h', 1, new Torre(Cor.Branca, Tab));
            ColocarNovaPeca('a', 2, new Peao(Cor.Branca, Tab, this));
            ColocarNovaPeca('b', 2, new Peao(Cor.Branca, Tab, this));
            ColocarNovaPeca('c', 2, new Peao(Cor.Branca, Tab, this));
            ColocarNovaPeca('d', 2, new Peao(Cor.Branca, Tab, this));
            ColocarNovaPeca('e', 2, new Peao(Cor.Branca, Tab, this));
            ColocarNovaPeca('f', 2, new Peao(Cor.Branca, Tab, this));
            ColocarNovaPeca('g', 2, new Peao(Cor.Branca, Tab, this));
            ColocarNovaPeca('h', 2, new Peao(Cor.Branca, Tab, this));

            ColocarNovaPeca('a', 8, new Torre(Cor.Preta, Tab));
            ColocarNovaPeca('b', 8, new Cavalo(Cor.Preta, Tab));
            ColocarNovaPeca('c', 8, new Bispo(Cor.Preta, Tab));
            ColocarNovaPeca('d', 8, new Dama(Cor.Preta, Tab));
            ColocarNovaPeca('e', 8, new Rei(Cor.Preta, Tab, this));
            ColocarNovaPeca('f', 8, new Bispo(Cor.Preta, Tab));
            ColocarNovaPeca('g', 8, new Cavalo(Cor.Preta, Tab));
            ColocarNovaPeca('h', 8, new Torre(Cor.Preta, Tab));
            ColocarNovaPeca('a', 7, new Peao(Cor.Preta, Tab, this));
            ColocarNovaPeca('b', 7, new Peao(Cor.Preta, Tab, this));
            ColocarNovaPeca('c', 7, new Peao(Cor.Preta, Tab, this));
            ColocarNovaPeca('d', 7, new Peao(Cor.Preta, Tab, this));
            ColocarNovaPeca('e', 7, new Peao(Cor.Preta, Tab, this));
            ColocarNovaPeca('f', 7, new Peao(Cor.Preta, Tab, this));
            ColocarNovaPeca('g', 7, new Peao(Cor.Preta, Tab, this));
            ColocarNovaPeca('h', 7, new Peao(Cor.Preta, Tab, this));
        }
    }
}
