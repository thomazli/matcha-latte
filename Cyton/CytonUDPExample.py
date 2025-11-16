# ...existing code...
import socket
import sys
import signal

def main():
    HOST = '127.0.0.1'
    PORT = 12345
    BUFSIZE = 65535

    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    try:
        sock.bind((HOST, PORT))
    except OSError as e:
        print(f"Failed to bind UDP socket to {HOST}:{PORT}: {e}")
        return

    print(f"Listening for UDP on {HOST}:{PORT} (press Ctrl+C to stop)")

    def _exit(signum, frame):
        print("Shutting down.")
        try:
            sock.close()
        finally:
            sys.exit(0)

    signal.signal(signal.SIGINT, _exit)

    while True:
        try:
            data, addr = sock.recvfrom(BUFSIZE)
        except OSError:
            break
        if not data:
            continue
        try:
            decoded = data.decode('utf-8')
        except Exception:
            decoded = data.decode('utf-8', errors='replace')
        print(f"From {addr}: {decoded!r}")

if __name__ == '__main__':
    main()
# ...existing code...