﻿'Skeleton Program for the AQA AS Paper 1 Summer 2016 examination
'this code should be used in conjunction with the Preliminary Material
'written by the AQA Programmer Team
'developed in the Visual Studio 2008 programming environment

'Version Number 1.0

Imports System.IO

Module Module1
    Const TrainingGame As String = "Training.txt"

    Structure TShip
        Dim Name As String
        Dim Size As Integer
    End Structure

    Sub GetRowColumn(ByRef Row As Integer, ByRef Column As Integer)
        Try
            Console.WriteLine()
            Console.Write("Please enter column: ")
            Column = Console.ReadLine()
            Console.Write("Please enter row: ")
            Row = Console.ReadLine()
            'Check for validation
            If (Row > 9) Or Row < 0 Or Column > 9 Or Column < 0 Then
                Console.WriteLine("Your coordinates are out of bound. Please reenter: ")
                GetRowColumn(Row, Column)
            End If

        Catch ex As Exception
            Console.WriteLine("Only numerical values are valid.")
            GetRowColumn(Row, Column)
        End Try
        Console.WriteLine()
    End Sub

    Sub MakePlayerMove(ByRef Board(,) As Char, ByRef Ships() As TShip)
        Dim Row As Integer
        Dim Column As Integer
        GetRowColumn(Row, Column)
        If Board(Row, Column) = "m" Or Board(Row, Column) = "h" Then
            Console.WriteLine("Sorry, you have already shot at the square (" & Column & "," & Row & "). Please try again.")
        ElseIf Board(Row, Column) = "-" Then
            Console.WriteLine("Sorry, (" & Column & "," & Row & ") is a miss.")
            Board(Row, Column) = "m"
        Else
            Console.WriteLine("Hit at (" & Column & "," & Row & ").")
            Board(Row, Column) = "h"
        End If

        Detectablity(Board, Row, Column)
    End Sub

    Private Sub Detectablity(ByVal Board(,) As Char, Hitrow As Integer, Hitcolumn As Integer)
        'For row = 0 To 9
        '    For column = 0 To 9
        '        If Not (Board(row, column) = "m" Or Board(row, column) = "h" Or Board(row, column) = "-") Then
        '            Dim dRow As Integer = row - Hitrow
        '            Dim dColumn As Integer = column - Hitcolumn
        '            If (dRow < 2 And dRow > -2 And dColumn > -2 And dColumn < 2) Then
        '                Console.WriteLine("DROW : " & dRow & " DCOLUMN : " & dColumn)
        '                Console.WriteLine("Ship detected in range of 1 cell.")
        '                Return
        '            End If
        '        End If
        '    Next
        'Next
        Dim pivotRow As Integer = Hitrow - 1
        Dim pivotColumn As Integer = Hitcolumn - 1
        Try
            For row = 0 To 2
                For column = 0 To 2
                    If Not (Board(pivotRow + row, pivotColumn + column) = "m" Or Board(pivotRow + row, pivotColumn + column) = "h" Or Board(pivotRow + row, pivotColumn + column) = "-") Then
                        Console.WriteLine("Ship detected in range of 1 cell.")
                        Return
                    End If
                Next
            Next
        Catch
        End Try
        Console.WriteLine("No ships detected in range.")
    End Sub

    Sub SetUpBoard(ByRef Board(,) As Char)
        Dim Row As Integer
        Dim Column As Integer
        For Row = 0 To 9
            For Column = 0 To 9
                Board(Row, Column) = "-"
            Next
        Next
    End Sub

    Sub LoadGame(ByVal Filename As String, ByRef Board(,) As Char)
        Dim Row As Integer
        Dim Column As Integer
        Dim Line As String
        Using FileReader As StreamReader = New StreamReader(Filename)
            For Row = 0 To 9
                Line = FileReader.ReadLine()
                For Column = 0 To 9
                    Board(Row, Column) = Line(Column)
                Next
            Next
        End Using
    End Sub

    Sub PlaceRandomShips(ByRef Board(,) As Char, ByVal Ships() As TShip)
        Dim Valid As Boolean
        Dim Row As Integer
        Dim Column As Integer
        Dim Orientation As Char
        Dim HorV As Integer
        For Each Ship In Ships
            Valid = False
            While Not Valid
                'Randomise seed
                Randomize()
                Row = Int(Rnd() * 10)
                Column = Int(Rnd() * 10)
                HorV = Int(Rnd() * 4)
                If HorV = 0 Then
                    Orientation = "v"
                ElseIf HorV = 1 Then
                    Orientation = "h"
                ElseIf HorV = 2 Then
                    Orientation = "p"
                ElseIf HorV = 3 Then
                    Orientation = "n"
                End If
                Valid = ValidateBoatPosition(Board, Ship, Row, Column, Orientation)
            End While
            Console.WriteLine("Computer placing the " & Ship.Name)
            PlaceShip(Board, Ship, Row, Column, Orientation)
        Next
    End Sub

    Sub PlaceShip(ByRef Board(,) As Char, ByVal Ship As TShip, ByVal Row As Integer, ByVal Column As Integer, ByVal Orientation As Char)
        Dim Scan As Integer
        If Orientation = "v" Then
            For Scan = 0 To Ship.Size - 1
                Board(Row + Scan, Column) = Ship.Name(0)
            Next
        ElseIf Orientation = "h" Then
            For Scan = 0 To Ship.Size - 1
                Board(Row, Column + Scan) = Ship.Name(0)
            Next
        ElseIf (Orientation = "p") Then
            For Scan = 0 To Ship.Size - 1
                Board(Row - Scan, Column + Scan) = Ship.Name(0)
            Next
        ElseIf (Orientation = "n") Then
            For Scan = 0 To Ship.Size - 1
                Board(Row + Scan, Column + Scan) = Ship.Name(0)
            Next
        End If
    End Sub

    Function ValidateBoatPosition(ByVal Board(,) As Char, ByVal Ship As TShip, ByVal Row As Integer, ByVal Column As Integer, ByVal Orientation As Char)
        Dim Scan As Integer
        If Orientation = "v" And Row + Ship.Size > 10 Then
            Return False
        ElseIf Orientation = "h" And Column + Ship.Size > 10 Then
            Return False
        ElseIf Orientation = "p"
            If Row - Ship.Size < -1 Or Column + Ship.Size > 10 Then
                Return False
            End If
        ElseIf Row + Ship.Size > 10 Or Column + Ship.Size > 10
            Return False
        Else
            If Orientation = "v" Then
                For Scan = 0 To Ship.Size - 1
                    If Board(Row + Scan, Column) <> "-" Then
                        Return False
                    End If
                Next
            ElseIf (Orientation = "h") Then
                For Scan = 0 To Ship.Size - 1
                    If Board(Row, Column + Scan) <> "-" Then
                        Return False
                    End If
                Next
            ElseIf (Orientation = "p") Then
                For Scan = 0 To Ship.Size - 1
                    If (Board(Row - Scan, Column + Scan) <> "-") Then
                        Return False
                    End If
                Next
            ElseIf (Orientation = "n") Then
                For Scan = 0 To Ship.Size - 1
                    If (Board(Row + Scan, Column + Scan) <> "-") Then
                        Return False
                    End If
                Next
            End If
        End If
        Return True
    End Function

    Function CheckWin(ByVal Board(,) As Char)
        Dim Row As Integer
        Dim Column As Integer
        For Row = 0 To 9
            For Column = 0 To 9
                If Board(Row, Column) = "A" Or Board(Row, Column) = "B" Or Board(Row, Column) = "S" Or Board(Row, Column) = "D" Or Board(Row, Column) = "P" Then
                    Return False
                End If
            Next
        Next
        Return True
    End Function

    Sub PrintBoard(ByVal Board(,) As Char)
        Dim Row As Integer
        Dim Column As Integer
        Console.WriteLine()
        Console.WriteLine("The board looks like this: ")
        Console.WriteLine()
        Console.Write(" ")
        For Column = 0 To 9
            Console.Write(" " & Column & "  ")
        Next
        Console.WriteLine()
        For Row = 0 To 9
            Console.Write(Row & " ")
            For Column = 0 To 9
                If Board(Row, Column) = "-" Then
                    Console.Write(" ")
<<<<<<< HEAD
<<<<<<< HEAD
                ElseIf Board(Row, Column) = "A" Or Board(Row, Column) = "C" Or Board(Row, Column) = "B" Or Board(Row, Column) = "S" Or Board(Row, Column) = "D" Or Board(Row, Column) = "P" Then
                    Console.Write(" ")
=======
                ElseIf Board(Row, Column) = "A" Or Board(Row, Column) = "B" Or Board(Row, Column) = "S" Or Board(Row, Column) = "D" Or Board(Row, Column) = "P" Then
                    '   Console.Write(" ")
                    Console.Write(Board(Row, Column))
>>>>>>> master
=======
                    'ElseIf Board(Row, Column) = "A" Or Board(Row, Column) = "C" Or Board(Row, Column) = "B" Or Board(Row, Column) = "S" Or Board(Row, Column) = "D" Or Board(Row, Column) = "P" Then
                    '    Console.Write(" ")
>>>>>>> Alternative_detectmethod
                Else
                    Console.Write(Board(Row, Column))
                End If
                If Column <> 9 Then
                    Console.Write(" | ")
                End If
            Next
            Console.WriteLine()
        Next
    End Sub

    Sub DisplayMenu()
        Console.WriteLine("MAIN MENU")
        Console.WriteLine()
        Console.WriteLine("1. Start new game")
        Console.WriteLine("2. Load training game")
        Console.WriteLine("9. Quit")
        Console.WriteLine()
    End Sub

    Function GetMainMenuChoice()
        Dim Choice As Integer
        Console.Write("Please enter your choice: ")
        Choice = Console.ReadLine()
        Console.WriteLine()
        Return Choice
    End Function

    Sub PlayGame(ByVal Board(,) As Char, ByVal Ships() As TShip)
        Dim GameWon As Boolean = False
        Do
            PrintBoard(Board)
            MakePlayerMove(Board, Ships)
            GameWon = CheckWin(Board)
            If GameWon Then
                Console.WriteLine("All ships sunk!")
                Console.WriteLine()
            End If
        Loop Until GameWon
    End Sub

    Sub SetUpShips(ByRef Ships() As TShip)
        Ships(0).Name = "Aircraft Carrier"
        Ships(0).Size = 5
        Ships(1).Name = "Battleship"
        Ships(1).Size = 4
        Ships(2).Name = "Submarine"
        Ships(2).Size = 3
        Ships(3).Name = "Destroyer"
        Ships(3).Size = 3
        Ships(4).Name = "Patrol Boat"
        Ships(4).Size = 2
        Ships(5).Name = "Cruiser"
        Ships(5).Size = 3
        Ships(6).Name = "Patrol Boat"
        Ships(6).Size = 2
        Ships(7).Name = "Patrol Boat"
        Ships(7).Size = 2
    End Sub

    Sub Main()
        Dim Board(9, 9) As Char
        Dim Ships(7) As TShip
        Dim MenuOption As Integer
        Do
            SetUpBoard(Board)
            SetUpShips(Ships)
            DisplayMenu()
            MenuOption = GetMainMenuChoice()
            If MenuOption = 1 Then
                PlaceRandomShips(Board, Ships)
                PlayGame(Board, Ships)
            ElseIf MenuOption = 2 Then
                LoadGame(TrainingGame, Board)
                PlayGame(Board, Ships)
            End If
        Loop Until MenuOption = 9
    End Sub
End Module
