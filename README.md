EntityClassGeneratorForOracle
=====

�e�[�u����`�ɑΉ�����Entity�N���X��������������R�}���h���C�����s�t�@�C���ł��B

## �g����

    C:\> EntityClassGeneratorForOracle.exe (namespace) (user)/(password)@(tnsname)

- namespace  
  ��������N���X�̖��O��Ԗ�
- user  
  �����Ώۃe�[�u���Q������Oracle Database�X�L�[�}�̃��[�U�[��
- user  
  �����Ώۃe�[�u���Q������Oracle Database�X�L�[�}�̃p�X���[�h
- tnsname  
  �����Ώۃe�[�u���Q������Oracle Database�T�[�o�[��TNS��

���s�t�@�C���Ɠ����t�H���_�[��`�e�[�u����.cs`�Ƃ��Đ�������܂��B

## ���s��
### ���s�R�}���h
    C:\> EntityClassGeneratorForOracle.exe MyApp.Entity SCOTT/TIGER@ORCL

### ���s���ʁiSCOTT�X�L�[�}��DEPT�e�[�u���j

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Entity
{
    public class DEPT
    {
        public Int32 DEPTNO { get; set; }
        public String DNAME { get; set; }
        public String LOC { get; set; }
 
        public override string ToString()
        {
            return ""
                + "DEPTNO : " + DEPTNO + " | "
                + "DNAME : " + DNAME + " | "
                + "LOC : " + LOC + " | "
                ;
        }
    }
}
```

## �K�v�v��

- .NET Framework 4.0 �ȍ~
- ODP.NET 11g R1 �ȍ~

## �Q�l����

- [neue cc - Micro-ORM�ƃe�[�u���̃N���X��`���������ɂ���](http://neue.cc/2013/06/30_411.html)