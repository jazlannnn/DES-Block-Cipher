# DES Block Cipher

## Simplified DES Application 

Introduction 
The Simplified DES Application is a software implementation of a basic version of the Data  Encryption Standard (DES), a symmetric-key block cipher originally developed in the 1970s. 
DES was widely used for encrypting and decrypting data, where the encryption process involves a series of permutations and substitutions using a secret key. 

For more detailed information about the DES algorithm and its historical significance, refer to this article: https://www.codeproject.com/Articles/91628/Simplified-version-of-the-DES-Data-Encryption-Stan 

This application is designed to replicate the core encryption and decryption process of DES,  with simplified operations and key scheduling. It allows users to interact with the system by providing an 8-bit 
binary input and key, enabling them to encrypt or decrypt messages using  a single round of the algorithm. 

The application allows the user to: 
- Input an 8-bit binary string for encryption or decryption. 
- Choose between encryption and decryption modes. 
- Encrypt or decrypt the message using the DES algorithm. 
- View the resulting encrypted or decrypted data after processing. 

### Output 
Example 1: Encryption & Decryption
![Screenshot 2024-12-09 165935](https://github.com/user-attachments/assets/68ef70ae-37d6-49c7-ae56-f6e6787e7cc5)

Example 2: Decryption & Encryption 
![Screenshot 2024-12-09 170033](https://github.com/user-attachments/assets/d977f172-85e1-4943-9db9-ad7ff1fe8422)


Conclusion 
The Simplified DES Application provides a straightforward way to understand the core principles of the Data Encryption Standard (DES) algorithm. By implementing key elements such as initial and final permutations, 
the Feistel function, and substitution operations, the application allows users to see how encryption and decryption processes work in a simplified manner. Although based on a reduced version of DES, it offers valuable insight 
into symmetric-key cryptography and can serve as an educational tool for those interested in learning the basics of block cipher encryption.


## DES Encryption Implementation 

Implementation Based on Example A 
This implementation follows the structure and requirements outlined in Example A to demonstrate the Data Encryption Standard (DES) algorithm in a detailed and interactive way. The application simulates the 
DES encryption process step-by-step, displaying intermediate results and final output as shown in Example A. 

Purpose: 
The purpose of this implementation is to encrypt a 64-bit plaintext message using the Data Encryption Standard (DES) algorithm. DES is a symmetric-key block cipher that encrypts data in 64-bit blocks using a 56-bit key.
This implementation mimics the operations described in Example A, displaying intermediate values and results to provide a clear understanding of the DES encryption process. 

Key Features: 
• Initial Permutation (IP): The plaintext undergoes an initial permutation to rearrange its bits. 
• Key Scheduling: A round key is generated for each of the 16 rounds by permuting and shifting the original key. 
• Rounds of Encryption: Each round involves expanding, XORing with the round key, substituting with S-boxes, and permuting the result. 
• Final Permutation (FP): After 16 rounds, the data is permuted again to produce the final ciphertext.


## Output 

Example 1: 

Plaintext: 0000000100100011010001010110011110001001101010111100110111101111 
Key: 0001001100110100010101110111100110011011101111001101111111110001


Example 2: 

Plaintext: 1101011010101011001110001110111010101100101011111001101110111011 
Key: 1010101010111011101110101011111110001001011100111110101010111101 


Example 3: 

Plaintext: 0110100101101111011100110110010101101001011100111101100101011100 
Key: 1101001101110110100101011000111000111010110011010100111010100011


Example 4: 

Plaintext: 1111000011110000111100001111000011110000111100001111000011110000 
Key: 0001100101011101001011001110101011101010010110010101001011111000 


Example 5: 

Plaintext: 1001101011011100111100101110101010111100100111001011001111101000 
Key: 1110001111001101011011101111110011100011111001001101111110001101



Conclusion 

This implementation successfully aligns with Example A, displaying the DES encryption process step-by-step for educational and verification purposes. It ensures that the plaintext, 
keys, intermediate states, and ciphertexts are represented in both binary and hexadecimal formats, making the process transparent and easy to follow.
