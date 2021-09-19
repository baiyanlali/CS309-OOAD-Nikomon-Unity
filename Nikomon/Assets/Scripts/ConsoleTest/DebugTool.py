import sys
import socket
import _thread

try:
    args=sys.argv[1:]
    port=int(args[0])
except Exception as e:
    print("Please input the port number")

class LogType:
    Error="#0"
    Assert="#1"
    Warning="#2"
    Log="#3"
    Exception="#4"


def parse_str(strs):
    print(strs)

def receive_input():
    while  True:
        strs=input()
        s.sendto(strs.encode('utf-8'),addr)

if __name__=="__main__":
    s=socket.socket(socket.AF_INET,socket.SOCK_DGRAM)
    s.bind(('',port))
    print("UDP Server Open, Port: "+args[0])
    is_init=False
    addr=0

    while True:
        data,client_addr=s.recvfrom(2048)
        addr=client_addr
        parse_str(data.decode(encoding='utf-8'))

        if not is_init:
            is_init=True
            _thread.start_new_thread(receive_input,())

