import React from 'react';
import TableContainer from '@mui/material/TableContainer';
import MuiTable from '@mui/material/Table';
import TableHead from '@mui/material/TableHead';
import TableBody from '@mui/material/TableBody';
import TableRow from '@mui/material/TableRow';
import TableCell from '@mui/material/TableCell';
import Paper from '@mui/material/Paper';

export type Column<T> = {
  key?: keyof T | string;
  header: React.ReactNode;
  align?: 'left' | 'center' | 'right';
  hidden?: boolean;
  minWidth?: number | string;
  render?: (row: T, rowIndex: number) => React.ReactNode;
};

export type TableProps<T> = {
  columns: Column<T>[];
  data: T[];
  maxHeight?: number | string;

  getRowKey?: (row: T, rowIndex: number) => React.Key;
};

export function Table<T>({ columns, data, maxHeight, getRowKey }: TableProps<T>) {
  return (
    <TableContainer
      component={Paper}
      sx={{
        maxHeight: maxHeight || 500,
        overflowX: 'auto',
        overflowY: 'auto',
        '&::-webkit-scrollbar:vertical': {
          width: 0,
          background: 'transparent',
        },
        '&::-webkit-scrollbar-thumb:vertical': {
          background: '#222',
          borderRadius: 4,
        },
        '&::-webkit-scrollbar:horizontal': {
          height: 8,
          background: 'transparent',
        },
        '&::-webkit-scrollbar-thumb:horizontal': {
          background: '#222',
          borderRadius: 8,
        },
        msOverflowStyle: 'auto', // IE and Edge
        scrollbarWidth: 'thin', // Firefox
        scrollbarColor: '#222 #111', // Firefox
      }}
    >
      <MuiTable size="small" aria-label="data table" stickyHeader sx={{ minWidth: 1000 }}>
        <TableHead>
          <TableRow
            sx={{
              backgroundColor: 'background.paper',
              boxShadow: 1,
              position: 'sticky',
              top: 0,
              zIndex: 2,
              opacity: 1,
            }}
          >
            {columns.map((col, idx) => (
              <TableCell
                key={col.key?.toString() ?? idx}
                align={col.align}
                sx={{
                  minWidth: col.minWidth,
                  display: col.hidden ? 'none' : undefined,
                  fontWeight: 'bold',
                  backgroundColor: 'background.paper',
                  color: 'text.primary',
                  boxShadow: 1,
                  borderBottom: '2px solid',
                  borderColor: 'divider',
                  paddingY: 1.5,
                  fontSize: '1rem',
                  letterSpacing: 0.5,
                }}
              >
                {col.header}
              </TableCell>
            ))}
          </TableRow>
        </TableHead>
        <TableBody>
          {data.map((row, rowIndex) => (
            <TableRow
              key={getRowKey ? getRowKey(row, rowIndex) : rowIndex}
              sx={{
                backgroundColor: 'grey.900',
                '&:hover': {
                  backgroundColor: 'action.hover',
                },
                transition: 'background 0.2s',
              }}
            >
              {columns.map((col, colIndex) => (
                <TableCell
                  key={col.key?.toString() ?? colIndex}
                  align={col.align}
                  sx={{
                    minWidth: col.minWidth,
                    display: col.hidden ? 'none' : undefined,
                    paddingY: 1.2,
                    fontSize: '0.97rem',
                    borderBottom: '1px solid',
                    borderColor: 'divider',
                  }}
                >
                  {col.render ? col.render(row, rowIndex) : col.key ? (row as any)[col.key] : null}
                </TableCell>
              ))}
            </TableRow>
          ))}
        </TableBody>
      </MuiTable>
    </TableContainer>
  );
}
export default React.memo(Table) as typeof Table;
