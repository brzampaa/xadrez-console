using System;
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

        private HashSet<Peca> Pecas;
        private HashSet<Peca> Capturadas;


        public PartidaXadrez() {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
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
            return pecaCapturada;
        }

        public void RealizaJogada(Posicao origem, Posicao destino) {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);
            if (EstaEmXeque(JogadorAtual)) {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            Xeque = (EstaEmXeque(Adversario(JogadorAtual))) ? true : false;

            if (TesteXequeMate(Adversario(JogadorAtual))) {
                Terminada = true;
            } else {
                Turno++;
                MudaJogador();
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
        }

        public void ValidarPosicaoOrigem(Posicao pos) {
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
            ColocarNovaPeca('h', 1, new Torre(Cor.Branca, Tab));
            ColocarNovaPeca('d', 1, new Rei(Cor.Branca, Tab));

            ColocarNovaPeca('a', 8, new Torre(Cor.Preta, Tab));
            ColocarNovaPeca('h', 8, new Torre(Cor.Preta, Tab));
            ColocarNovaPeca('d', 8, new Rei(Cor.Preta, Tab));
        }
    }
}
