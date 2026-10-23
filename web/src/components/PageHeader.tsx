type PageHeaderProps = {
  title: string;
  subtitle: string;
};

function PageHeader({ title, subtitle }: PageHeaderProps) {
  return (
    <header className="page-header">
      <h1>{title}</h1>
      <p className="subtitle">{subtitle}</p>
    </header>
  );
}

export default PageHeader;
