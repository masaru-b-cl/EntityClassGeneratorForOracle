EntityClassGeneratorForOracle
=====

テーブル定義に対応したEntityクラスを自動生成するコマンドライン実行ファイルです。

## 使い方

    C:\> EntityClassGeneratorForOracle.exe (namespace) (user)/(password)@(tnsname)

- namespace  
  生成するクラスの名前空間名
- user  
  生成対象テーブル群を持つOracle Databaseスキーマのユーザー名
- user  
  生成対象テーブル群を持つOracle Databaseスキーマのパスワード
- tnsname  
  生成対象テーブル群を持つOracle DatabaseサーバーのTNS名

実行ファイルと同じフォルダーに`テーブル名.cs`として生成されます。

## 実行例
### 実行コマンド
    C:\> EntityClassGeneratorForOracle.exe MyApp.Entity SCOTT/TIGER@ORCL

### 実行結果（SCOTTスキーマのDEPTテーブル）

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

## 必要要件

- .NET Framework 4.0 以降
- ODP.NET 11g R1 以降

## 参考資料

- [neue cc - Micro-ORMとテーブルのクラス定義自動生成について](http://neue.cc/2013/06/30_411.html)