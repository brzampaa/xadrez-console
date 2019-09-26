using System;

namespace tabuleiro.Exceptions {
    class TabuleiroException : ArgumentException {

        public TabuleiroException(string msg) : base(msg) {
        }
    }
}
