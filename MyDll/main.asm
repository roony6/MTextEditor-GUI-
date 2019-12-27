INCLUDE Irvine32.inc
INCLUDE macros.inc

;DllMain PROTO, hInstance:DWORD, fdwReason:DWORD, lpReserved:DWORD

;WriteCreateFile proto, FileN: ptr byte, StringSize: Dword, Text: ptr byte
;MyReadFromFile Proto, FileNa: ptr byte, Txt: ptr byte

.DATA
	BUFFER_SIZE = 5000

	buffer BYTE BUFFER_SIZE DUP(?)	
	filename byte 20 dup(?), 0
	fileHandle HANDLE ?
	stringLength DWORD ?
	str1 BYTE "Cannot create file",0dh,0ah,0
	strr byte 5000 dup(?), 0
.code

WriteCreateFile Proc, FileN: ptr byte, StringSize: Dword, Text: ptr byte

; Create a new text file.
	mov edx, FileN
	call CreateOutputFile
	mov fileHandle,eax

; Check for errors.
	cmp eax, INVALID_HANDLE_VALUE ; error found?
	jne file_ok ; no: skip
	mov edx,OFFSET str1 ; display error
	call WriteString
	jmp quit

file_ok:
; Write the buffer to the output file.
	mov eax,fileHandle
	mov edx, Text
	mov ecx,StringSize
	call WriteToFile
	mov StringSize,eax ; save return value
	call CloseFile

quit:

	ret
WriteCreateFile Endp


MyReadFromFile Proc, FileNa: ptr byte, Txt: ptr byte

; Open the file for input.
	mov	edx, FileNa
	call OpenInputFile
	mov	fileHandle,eax

; Check for errors.
	cmp	eax,INVALID_HANDLE_VALUE		; error opening file?
	jne	file_ok					; no: skip
	mWrite <"Cannot open file",0dh,0ah>
	jmp	quit	
						; and quit
file_ok:
; Read the file into a buffer.
	mov	edx, Txt
	mov	ecx, BUFFER_SIZE
	call ReadFromFile
	jnc	check_buffer_size			; error reading?
	mWrite "Error reading file. "		; yes: show error message
	call	WriteWindowsMsg
	jmp	close_file
	
check_buffer_size:
	cmp	eax,BUFFER_SIZE			; buffer large enough?
	jb	buf_size_ok				; yes
	mWrite <"Error: Buffer too small for the file",0dh,0ah>
	jmp	quit						; and quit
	
buf_size_ok:
	
	mov	txt[eax],0		; insert null terminator
	mWrite "File size: "
	call	WriteDec			; display file size
	call	Crlf

close_file:
	mov	eax,fileHandle
	call	CloseFile


quit:
	ret
MyReadFromFile ENDP

; DllMain is required for any DLL
DllMain PROC, hInstance:DWORD, fdwReason:DWORD, lpReserved:DWORD
mov eax, 1 ; Return true to caller.

exit
DllMain ENDP
END DllMain

