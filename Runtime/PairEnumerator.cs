// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Scheme
{
    [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    public struct PairEnumerator : IEnumerator<SchemeObject>
    {
        private enum State { Reset, Enumerating, Finished }

        private readonly Pair pair;
        private Pair currentPair;
        private State state;

        public PairEnumerator(Pair pair)
        {
            this.pair = pair;
            this.currentPair = null;
            this.state = State.Reset;
        }

        public SchemeObject Current
        {
            get { return this.CurrentPair.Car; }
        }

        public Pair CurrentPair
        {
            get
            {
                switch (this.state)
                {
                    case State.Reset:
                    default:
                        throw new InvalidOperationException();

                    case State.Enumerating:
                        return this.currentPair;
                }
            }
        }

        public SchemeObject FinalCdr
        {
            get
            {
                switch (this.state)
                {
                    case State.Reset:
                    case State.Enumerating:
                    default:
                        throw new InvalidOperationException();

                    case State.Finished:
                        return this.currentPair.Cdr;
                }
            }
        }

        public bool MoveNext()
        {
            switch (this.state)
            {
                case State.Reset:
                    this.currentPair = pair;
                    this.state = State.Enumerating;
                    return true;

                case State.Enumerating:
                    SchemeObject cdr = this.currentPair.Cdr;
                    Pair cdrPair = cdr as Pair;
                    if (cdrPair == null)
                    {
                        this.state = State.Finished;
                        return false;
                    }
                    this.currentPair = cdrPair;
                    return true;

                case State.Finished:
                default:
                    return false;
            }
        }

        public bool Finished
        {
            get
            {
                return this.state == State.Finished;
            }
        }

        public void Reset()
        {
            this.currentPair = null;
            this.state = State.Reset;
        }

        public void Dispose()
        {
        }

        object IEnumerator.Current
        {
            get { return this.Current; }
        }
    }
}