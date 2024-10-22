﻿using System;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace PrinterChangeNotification.enums
{
    [Flags]
    public enum PRINTER_CHANGE : UInt32
    {
        PRINTER_CHANGE_ADD_PRINTER              =0x00000001,
        PRINTER_CHANGE_SET_PRINTER              =0x00000002,
        PRINTER_CHANGE_DELETE_PRINTER           =0x00000004,
        PRINTER_CHANGE_FAILED_CONNECTION_PRINTER=0x00000008,
        PRINTER_CHANGE_PRINTER                  =0x000000FF,
        PRINTER_CHANGE_ADD_JOB                  =0x00000100,
        PRINTER_CHANGE_SET_JOB                  =0x00000200,
        PRINTER_CHANGE_DELETE_JOB               =0x00000400,
        PRINTER_CHANGE_WRITE_JOB                =0x00000800,
        PRINTER_CHANGE_JOB                      =0x0000FF00,
        PRINTER_CHANGE_ADD_FORM                 =0x00010000,
        PRINTER_CHANGE_SET_FORM                 =0x00020000,
        PRINTER_CHANGE_DELETE_FORM              =0x00040000,
        PRINTER_CHANGE_FORM                     =0x00070000,
        PRINTER_CHANGE_ADD_PORT                 =0x00100000,
        PRINTER_CHANGE_CONFIGURE_PORT           =0x00200000,
        PRINTER_CHANGE_DELETE_PORT              =0x00400000,
        PRINTER_CHANGE_PORT                     =0x00700000,
        PRINTER_CHANGE_ADD_PRINT_PROCESSOR      =0x01000000,
        PRINTER_CHANGE_DELETE_PRINT_PROCESSOR   =0x04000000,
        PRINTER_CHANGE_PRINT_PROCESSOR          =0x07000000,
        PRINTER_CHANGE_SERVER                   =0x08000000,
        PRINTER_CHANGE_ADD_PRINTER_DRIVER       =0x10000000,
        PRINTER_CHANGE_SET_PRINTER_DRIVER       =0x20000000,
        PRINTER_CHANGE_DELETE_PRINTER_DRIVER    =0x40000000,
        PRINTER_CHANGE_PRINTER_DRIVER           =0x70000000,
        PRINTER_CHANGE_TIMEOUT                  =0x80000000,
        PRINTER_CHANGE_ALL                      =0x7F77FFFF,
    }
}