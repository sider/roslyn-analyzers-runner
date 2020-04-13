# -*- coding: utf-8 -*-
import numpy as np

if __name__ == '__main__':
    A = np.array([[1.,0.]
                 ,[0.,2.]])
    invA = np.linalg.inv(A)
    print("invA=", str(invA))
