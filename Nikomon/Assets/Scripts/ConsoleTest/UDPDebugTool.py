import socket
import time
import _thread


def send():
    while True:
        server.sendto('broadcast'.encode('UTF-8'), ('255.255.255.255', 42424))
        time.sleep(2)
        print('Send over')


if __name__ == '__main__':
    PORT = 42425

    server = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

    server.setsockopt(socket.SOL_SOCKET,socket.SO_BROADCAST,1)
    # address = ('', PORT)
    # server.bind(address)
    # server.settimeout(20)
    print('Start to server')

    _thread.start_new_thread(send())
    # server.sendto(str.encode('hello'),('<broadcast>',42424))

    # server.sendto('hello'.encode('UTF-8'),('127.0.0.1',42424))
    # _thread.start_new_thread(send())
    # while True:
    #     receive_data,client=server.recvfrom(2048)
    #     print('From client:',client,' and is ',receive_data)
