import mysql.connector
import socket
import sys
import time
from threading import Thread
from queue import Queue

# Create a TCP/IP socket
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
print('Socket created!\n')

# Bind the socket to the port
server_address = ('192.168.1.157', 10000)
sock.bind(server_address)
print('Socket binded!\n')

# Listen for incoming connections
sock.listen(1)
print('Socket listening!\n')
                
# Player Pool thread
def pPool(in_q):
    while True:
        # Get some data
        data = in_q.get()
        print('data received!')
        data.send(b's')
        # Process the data
                
if __name__ == "__main__":

  print('welcome\n')

  print('create queue\n')  
  q = Queue()
  print('queue created, create pool thread')
  t1 = Thread(target=pPool, args=(q,))
  print('thread created, start it')
  t1.start()
  print('thread started, go on with the db')
  
  cnx = mysql.connector.connect(user='root', password='Isocomp_15', host='127.0.0.1', database='test')
  cnx.autocommit = True
  x = cnx.cursor(buffered=True)
  
  while True:
      # Wait for a connection
      print('waiting for a connection')
      connection, client_address = sock.accept()
      
      try:
        print('connection from', client_address)
    
        # Receive the data in small chunks and retransmit it
        while True:
            data = connection.recv(1400)
            print('received ', data)
            stri = data.decode("utf-8")
            print('converted to ', stri)
            uName, pWord = stri.split(";")
            print('username: ', uName)
            print('password: ', pWord)
            x.execute("SELECT password FROM users WHERE username = '%s'" %(uName))
            retu = x.fetchone()
            ulti = retu[0]
            print('returned: ', ulti)
            if(ulti == None):
              print ('Nome utente o password non validi')
              x.close()
            else:
              if ulti == pWord:
                  print('Connection success')
                  #connection.send(b's')
                  q.put(connection)
                  time.sleep(0.1)
                  break
              else:
                  print('Connection fail')
                  connection.send(b'f')
                  break
                
      finally:
          # Clean up the connection
          connection.close()