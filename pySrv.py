import mysql.connector
import socket
import sys

print('welcome\n')

cnx = mysql.connector.connect(user='root', password='Isocomp_15', host='127.0.0.1', database='test')
cnx.autocommit = True
x = cnx.cursor(buffered=True)

# Create a TCP/IP socket
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
print('Socket created!\n')

# Bind the socket to the port
server_address = ('192.168.1.157', 10000)
#sys.stderr.write('starting up on %s port %s' % server_address)
sock.bind(server_address)
print('Socket binded!\n')

# Listen for incoming connections
sock.listen(1)
print('Socket listening!\n')

while True:
    # Wait for a connection
    #sys.stderr.write('waiting for a connection')
    print('waiting for a connection')
    connection, client_address = sock.accept()
    
    try:
      #sys.stderr.write('connection from', client_address)
      print('connection from', client_address)
  
      # Receive the data in small chunks and retransmit it
      while True:
          data = connection.recv(1400)
          #sys.stderr.write('received "%s"' % data)
          #print('received "%s"' % data)
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
                #sys.stderr.write('sending data back to the client')
                print('Connection success')
                connection.send(b's')
                break
            else:
                print('Connection fail')
                connection.send(b'f')
                break
              
    finally:
        # Clean up the connection
        connection.close()