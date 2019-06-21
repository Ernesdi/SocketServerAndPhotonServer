using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MyGameServer.Event;

namespace MyGameServer.Threads
{
    public class SyncPos
    {
        SyncPositionEvent syncPositionEvent = new SyncPositionEvent();

        private ClientPeer clientPeer;
        private Thread thread;

        public SyncPos(ClientPeer clientPeer)
        {
            this.clientPeer = clientPeer;
            thread = new Thread(Run);
            thread.Start();
        }

        private void Run()
        {
            Thread.Sleep(500);
            while (true)
            {
                syncPositionEvent.SendPosExcuteSelf(clientPeer);
                Thread.Sleep(50);
            }

        }

        public void StopRun()
        {
            thread.Abort();
        }

    }
}
