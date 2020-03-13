# -*- coding: utf-8 -*-
import numpy as np

def main():
    A = np.array([[1.,0.]
                 ,[0.,2.]])
    invA = np.linalg.inv(A)
    print("invA=", str(invA))

if __name__ == '__main__':
    main()
