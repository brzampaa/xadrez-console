﻿using xadrez_console.Tabuleiro;
using xadrez_console.Tabuleiro.Enums;

namespace xadrez_console.Tabuleiro {
    class Peca {
        public Posicao Posicao { get; set; }
        public Cor Cor { get; protected set; }
        public int QteMovimentos { get; protected set; }
        public Tabuleiro Tab { get; set; }

        public Peca(Posicao posicao, Cor cor, Tabuleiro tab) {
            Posicao = posicao;
            Cor = cor;
            Tab = tab;
            QteMovimentos = 0;
        }
    }
}
