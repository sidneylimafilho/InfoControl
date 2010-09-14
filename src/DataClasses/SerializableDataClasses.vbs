Dim FileName, Find, ReplaceWith, FileContents
FileName     = WScript.Arguments(0)


'Read source text file
FileContents = GetFile(FileName)

'replace all string In the source file
'FileContents = replace(FileContents, "Serializable, ", "", 1, -1, 1)
'FileContents = replace(FileContents, "System.Runtime.Serialization.DataMember, ", "", 1, -1, 1)
FileContents = replace(FileContents, "[NonSerialized]", "", 1, -1, 1)


'FileContents = replace(FileContents, "global::System.Data.Linq.Mapping.TableAttribute", _
'                                     "Serializable, global::System.Data.Linq.Mapping.TableAttribute", 1, -1, 1)
'FileContents = replace(FileContents, "global::System.Data.Linq.Mapping.ColumnAttribute",  _ 
'                                     "System.Runtime.Serialization.DataMember, global::System.Data.Linq.Mapping.ColumnAttribute", 1, -1, 1)
FileContents = replace(FileContents, "private EntityRef", "[NonSerialized] "& vbCrLf &" private EntityRef", 1, -1, 1)
FileContents = replace(FileContents, "private EntitySet", "[NonSerialized] "& vbCrLf &" private EntitySet", 1, -1, 1)




  'write result If different
  WriteFile FileName, FileContents


'Read text file
function GetFile(FileName)
  If FileName<>"" Then
    Dim FS, FileStream
    Set FS = CreateObject("Scripting.FileSystemObject")
      on error resume Next
      Set FileStream = FS.OpenTextFile(FileName)
      GetFile = FileStream.ReadAll
  End If
End Function

'Write string As a text file.
function WriteFile(FileName, Contents)
  Dim OutStream, FS

  on error resume Next
  Set FS = CreateObject("Scripting.FileSystemObject")
    Set OutStream = FS.OpenTextFile(FileName, 2, True)
    OutStream.Write Contents
End Function