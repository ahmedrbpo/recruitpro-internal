import type { ReactNode } from 'react'

interface Column<T> {
  key: string
  header: string
  render: (row: T) => ReactNode
}

interface DataTableProps<T> {
  columns: Column<T>[]
  rows: T[]
  rowKey: (row: T) => string
  emptyMessage?: string
}

export function DataTable<T>({ columns, rows, rowKey, emptyMessage = 'No records found.' }: DataTableProps<T>) {
  return (
    <table className="min-w-full divide-y divide-slate-200 text-sm">
      <thead className="bg-brand-50/60">
        <tr>
          {columns.map((column) => (
            <th key={column.key} className="px-3 py-2 text-left font-semibold text-slate-600">
              {column.header}
            </th>
          ))}
        </tr>
      </thead>
      <tbody className="divide-y divide-slate-100">
        {rows.length === 0 ? (
          <tr>
            <td colSpan={columns.length} className="px-3 py-6 text-center text-slate-400">
              {emptyMessage}
            </td>
          </tr>
        ) : (
          rows.map((row) => (
            <tr key={rowKey(row)} className="hover:bg-brand-50/40">
              {columns.map((column) => (
                <td key={column.key} className="px-3 py-2 text-slate-800">
                  {column.render(row)}
                </td>
              ))}
            </tr>
          ))
        )}
      </tbody>
    </table>
  )
}
